using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBoom : MonoBehaviour
{
    // Singleton
    public static WallBoom instance {get; private set;}

    // Audio
    AudioSource audioSource;

    void Awake(){
        instance = this;
    }

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBoom(){
        audioSource.Play();
    }

}
