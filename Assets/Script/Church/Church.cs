using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Church : MonoBehaviour
{   
    AudioSource audioSource;
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {   
        if (!audioSource.isPlaying && other.CompareTag("Player"))
        {
            audioSource.Play();    
        }
    }

    void OnTriggerExit(Collider other)
    {
    
         if (audioSource.isPlaying && other.CompareTag("Player"))
        {
            audioSource.Stop();    
        }  
    }
}
