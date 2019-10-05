using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // Singleton
    public static ShopManager instance {get; private set;}

    // All objects that are related to the shop
    GameObject[] shopObjects;
    // All objects that are related to gameover
    GameObject[] gameOverObjects;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        shopObjects = GameObject.FindGameObjectsWithTag("ShopScreen");
        hideShop();

        gameOverObjects = GameObject.FindGameObjectsWithTag("GameOverScreen");
        foreach (GameObject obj in gameOverObjects)
        {
            obj.SetActive(false);
        }

    }

    public void startShop()
    {
        // Pause the game
        Time.timeScale = 0;
        showShop();
    }

    public void endShop()
    {
        // Resume the game
        hideShop();
        Time.timeScale = 1;
    }


    void showShop()
    {
        foreach (GameObject obj in shopObjects)
        {
            obj.SetActive(true);
        }
    }

    void hideShop()
    {
        foreach (GameObject obj in shopObjects)
        {
            obj.SetActive(false);
        }
    }

    public void EndGame(){
        // End the game
        foreach (GameObject obj in gameOverObjects)
        {
            obj.SetActive(true);
        }    
    }


}
