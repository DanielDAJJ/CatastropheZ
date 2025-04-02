using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatFollow_2 : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;
    public Animator aiAnim;
    public float followDistance = 2f;  // Distancia a la izquierda del jugador

    void Start()
    {
        player=GameObject.Find("Female Player").transform;
    }

    void Update()
    {
        // Calcula la direcci�n a la izquierda del jugador
        Vector3 leftOffset = -player.right * followDistance;
        Vector3 targetPosition = player.position + leftOffset;

        // Asignar como destino al NavMeshAgent
        ai.destination = targetPosition;

        // Cambiar animaci�n seg�n la distancia restante
        if (ai.remainingDistance <= ai.stoppingDistance)
        {
            aiAnim.ResetTrigger("jog");
            aiAnim.SetTrigger("idle");
        }
        else
        {
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("jog");
        }
    }
}
