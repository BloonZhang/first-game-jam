using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnController : MonoBehaviour
{
    // Singleton
    public static SpawnController instance {get; private set;}

    // Arrays
    public GameObject[] enemyArray;
    public GameObject[] collectableArray;

    // Public variables
    //public float enemySpawnTime = 3.0f;
    //public float collectableSpawnTime = 2.0f;

    // Properties
    public int Level {get {return currentLevel;}}
    public Text levelText;

    // Private variables
    int currentLevel = 1;
    float level1Freq;   // y = 2x
    float level2Freq;   // y = 2x^1.2 - 3
    float level3Freq;   // y = 2x^1.3 - 6
    float level4Freq;   // y = 3x^1.4 - 30
    float level5Freq;   // y = 3x^1.6 - 70
    float enemySpawnTime;    // y = 3/x^0.5
    float HPSpawnTime;  // y = 3.0 - 0.2 * x, down to 1.0

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
        LevelUp();
    }

    public void LevelUp()
    {
        currentLevel += 1;
        levelText.text = "Level: " + currentLevel;
        CancelInvoke();
        if (currentLevel != 1)
        {
            ShopManager.instance.startShop();
        }
        SpawnLevelFreqs(currentLevel);
        InvokeRepeating("SpawnEnemyDecider", enemySpawnTime, enemySpawnTime);
        InvokeRepeating("SpawnHP", HPSpawnTime, HPSpawnTime);
    }

    void SpawnEnemyDecider(){
        // Enemies
        // Add up the total for enemies
        float total = 0.0f;
        float[] probVector = {level1Freq, level2Freq, level3Freq, level4Freq, level5Freq};
        Debug.Log(probVector);
        foreach (float freq in probVector){total += freq;}

        // Random number
        float rand = Random.Range(0.0f, total);

        // Find the correct spawn
        for (int i = 0; i < probVector.Length; i++){
            if (rand < probVector[i]) {
                SpawnEnemy(i);
                return;
            }
            else{
                rand -= probVector[i];
            }
        }
        SpawnEnemy(0);
    }

    void SpawnEnemy(int enemyType){
        // Random location here away from player
        Vector2 randVector;
        do 
        {
            randVector = new Vector2(Random.Range(-12, 10), Random.Range(-8, 9)); 
        } while (Mathf.Abs(Vector2.Distance(randVector, PlayerController.instance.GetPlayerPosition())) < 6.0f);

        Instantiate(enemyArray[enemyType], randVector, Quaternion.identity);
    }

    void SpawnHP(){
        // Random number and location
        float rand = Random.Range(0.0f, 10f);
        Vector2 randVector;
        do 
        {
            randVector = new Vector2(Random.Range(-12, 10), Random.Range(-8, 9)); 
        } while (Mathf.Abs(Vector2.Distance(randVector, PlayerController.instance.GetPlayerPosition())) < 6.0f);

        // Make big hp packs 10% more common each level
        if (rand < currentLevel)
        {
            // Spawn big hp pack
            Instantiate(collectableArray[1], randVector, Quaternion.identity);
        }
        else
        {
            // Spawn little hp pack
            Instantiate(collectableArray[0], randVector, Quaternion.identity);
        }
    }

    void SpawnLevelFreqs(int level)
    {
        level1Freq = level;
        level2Freq = Mathf.Max(2, 2 * Mathf.Pow(level, 1.2f) - 3);
        level3Freq = Mathf.Max(0, 2 * Mathf.Pow(level, 1.3f) - 6);
        level4Freq = Mathf.Max(0, 3 * Mathf.Pow(level, 1.4f) - 30);
        level5Freq = Mathf.Max(0, 3 * Mathf.Pow(level, 1.6f) - 70);
        enemySpawnTime = 3.0f / Mathf.Pow(level, 0.5f);
        HPSpawnTime = Mathf.Max(3.0f - 0.3f * currentLevel, 1.0f);
    }
}
