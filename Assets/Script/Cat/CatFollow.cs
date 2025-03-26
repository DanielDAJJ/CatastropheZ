using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatFollow : MonoBehaviour
{
    // Referencia al personaje
    public Transform target;
    // Offset relativo (ejemplo: 2 unidades a la izquierda y 2 atrás)
    public Vector3 offset = new Vector3(-2f, 0f, -2f);
    // Distancia mínima para que el gato se detenga
    public float stopDistance = 0.1f;

    // Umbral de movimiento del personaje para considerar que se está moviendo
    public float movementThreshold = 0.0001f;

    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 lastTargetPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (target == null)
            Debug.LogError("Target no asignado en CatFollowAnimation.");

        // Guardamos la posición inicial del personaje
        lastTargetPosition = target.position;
    }

    void Update()
    {
        // Calcula la posición deseada usando el offset relativo a la orientación del personaje
        Vector3 desiredPos = target.position + target.TransformDirection(offset);
        float distance = Vector3.Distance(transform.position, desiredPos);

        // Mueve al gato si está lejos del punto deseado
        if (distance > stopDistance)
        {
            agent.SetDestination(desiredPos);
        }
        else
        {
            agent.ResetPath();
        }

        // Calcula cuánto se ha movido el personaje desde el último frame
        float targetMovement = Vector3.Distance(target.position, lastTargetPosition);

        // Si el personaje se mueve por encima del umbral, activamos run; de lo contrario, idle
        if (targetMovement > movementThreshold)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Actualizamos la posición anterior del personaje para la siguiente comparación
        lastTargetPosition = target.position;
    }
}
