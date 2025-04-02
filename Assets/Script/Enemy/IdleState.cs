using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    PersuitTargetState persuitTargetState;

   
    [Header("Detection Radius")]
    [SerializeField] float detectionRadius = 50f;

    [Header("Detection Layer")]
    [SerializeField] LayerMask detectionLayer;

    [Header("Detection Angle")]
    [SerializeField] float minimumDetectionRadiusAngle = -50f;
    [SerializeField] float maximumDetectionRadiusAngle = 50f;

    [Header("Zombie Eye Height")]
    [SerializeField] float zombieEyeHeight = 1.8f;

    // Altura a la que queremos apuntar del jugador (ej. pecho/cabeza)
    [Header("Player Target Height")]
    [SerializeField] float playerTargetHeight = 1.0f;

    [SerializeField ]CarController carController;


    private void Awake()
    {
        persuitTargetState = GetComponent<PersuitTargetState>();
        carController = GameObject.Find("Monster Car").GetComponent<CarController>();
        
    }
    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget != null)
        {            
            return persuitTargetState;
        }

        else
        {
            FindTargetViaOfSight(zombieManager);
            ListenEngineCar(zombieManager);
            return this;
        }
            
    }

    private void ListenEngineCar(ZombieManager zombieManager)
    {
        if (carController.isInCar)
        {
            zombieManager.currentTarget=carController.gameObject.transform;
        }
        
               
    }

    private void Update()
    {
        // Dibuja una l�nea azul desde la posici�n del zombie en la direcci�n de su forward, 5 unidades de largo.
       // Debug.DrawRay(transform.position, transform.forward * 5, Color.blue, 0f);
    }




    private void FindTargetViaOfSight(ZombieManager zombieManager)
    {
        zombieManager.currentTarget = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        for ( int i = 0; i < colliders.Length; i++)
        {
           if (colliders[i].CompareTag("Player"))
           {
                Transform playerTransform = colliders[i].transform;
                Vector3 targetDirection = playerTransform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                //Debug.Log("viewableAngle = " + viewableAngle);

                if (viewableAngle >= minimumDetectionRadiusAngle && viewableAngle <= maximumDetectionRadiusAngle)
                {
                    Vector3 zombieEyePosition = transform.position + Vector3.up * zombieEyeHeight;

                    Vector3 playerTargetPosition = playerTransform.position + Vector3.up * playerTargetHeight;

                    Vector3 directionToPlayer = (playerTargetPosition - zombieEyePosition).normalized;
                    float distanceToPlayer = Vector3.Distance(zombieEyePosition, playerTargetPosition);


                    if (Physics.Raycast(zombieEyePosition, directionToPlayer, out RaycastHit hit, distanceToPlayer, detectionLayer))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            // Dibujamos una l�nea verde si la vista no est� bloqueada
                         //   Debug.DrawLine(zombieEyePosition, playerTargetPosition, Color.green, 999999f);

                            zombieManager.currentTarget = playerTransform;
                       //     Debug.Log($"Jugador en mi �ngulo de visi�n: {viewableAngle}�");
                            break;

                        }
                        else
                        {
                            // Dibujamos una l�nea roja si golpeamos otro objeto
                     //       Debug.DrawLine(zombieEyePosition, hit.point, Color.red, 0.5f);

                    //        Debug.Log("Jugador est� en �ngulo, pero hay un objeto bloqueando la vista.");
                        }
                    }
                    else
                    {
                        // Si no golpeamos nada, podr�amos dibujar una l�nea de otro color
                        Debug.DrawLine(zombieEyePosition, playerTargetPosition, Color.yellow, 0.5f);

                        // Si el ray no golpea nada (lo cual es raro si el jugador est� en el radio),
                        // podr�as asumir que no hay bloqueo, o manejarlo de otra forma
                  //      Debug.Log("Raycast no golpe� nada. Posible error en la capa o distancia.");

                    }


                    
                }
                else
                {
                //    Debug.Log($"Jugador detectado, pero fuera del �ngulo de visi�n: {viewableAngle}�");
                }
           }


        }

    }
}
