using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSoundStart : MonoBehaviour
{   
    bool firstTimeSound;
    void OnTriggerEnter(Collider other)
    {   
        if(other.CompareTag("Player") && !firstTimeSound)
        {   
            AudioManager.instance.PlaySound(AudioManager.instance.c_solo);
            firstTimeSound=false;
            Destroy(this.gameObject,3f);
        }
        
    }


}
