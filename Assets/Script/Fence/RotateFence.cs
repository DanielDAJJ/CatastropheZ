using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateFence : MonoBehaviour
{
     [SerializeField] bool isOpen;   
     [SerializeField] bool playerNear;   
     [SerializeField] float distanceSqr;   
    void Start()
    {
        isOpen=false;
    }

    void FixedUpdate()
    {
        playerNear = Physics.CheckSphere(transform.position, 3.0f, LayerMask.GetMask("Player"));

        // Imprimir el resultado
        if (playerNear)
        {
            transform.rotation=Quaternion.Euler(0f,95f,0f);
        }
        else 
        {
            transform.rotation=Quaternion.Euler(0f,0f,0f);
           
        }

      
    }
  
    
}
