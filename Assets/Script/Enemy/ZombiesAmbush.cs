using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombiesAmbush : MonoBehaviour
{   
    private Collider boxCollider;
    private GameObject zombiesDeadBodies;
    private GameObject zombiesAttack;
    [SerializeField] AudioSource audioSource;
    
    
    void Start()
    {
        boxCollider= GetComponent<BoxCollider>();
        zombiesDeadBodies=transform.Find("ZombiesDeadBodies").gameObject;
        zombiesAttack=transform.Find("ZombiesAttack").gameObject;
    }

  
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boxCollider.enabled=false;
            zombiesDeadBodies.SetActive(false);
            zombiesAttack.SetActive(true);
            audioSource.Play(); 
        }
    }

}
