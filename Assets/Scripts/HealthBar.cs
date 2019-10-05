using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Singleton
    public static HealthBar instance {get; private set;}

    // Audio

    public Image mask;
    //float maxSize;
    float currentValue;
    float levelUp;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //maxSize = mask.rectTransform.rect.width / 2; //Divide by two for low level
        levelUp = mask.rectTransform.rect.width;
        //currentValue = maxSize;
        //mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxSize);
        currentValue = levelUp / 2.0f;
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentValue);
    }

    public void changeValue(float value)
    {
        currentValue += value;
        //currentValue = Mathf.Clamp(currentValue, 0, maxSize);
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentValue);

        
        if (currentValue >= levelUp){
            currentValue = levelUp / 2.0f;
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentValue);
            SpawnController.instance.LevelUp();
        }
        

     
        if (currentValue <= 0){
            PlayerController.instance.GameOver();
        }
        

    }


    public bool buyUpgrade(){
        if (currentValue >= 0.225f * levelUp){
            currentValue -= levelUp * 0.2f;
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentValue);
            return true;
        }
        return false;
    }

    public bool buyDowngrade(){
        if (currentValue <= 0.775f * levelUp){
            currentValue += levelUp * 0.2f;
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentValue);
            return true;
        }
        return false;    }


}
