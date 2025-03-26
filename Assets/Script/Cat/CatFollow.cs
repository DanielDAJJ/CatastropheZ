using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatFollow : MonoBehaviour
{
    // Referencia al personaje
    public Transform target;
    // Offset relativo (ejemplo: 2 unidades a la izquierda y 2 atr�s)
    public Vector3 offset = new Vector3(-2f, 0f, -2f);
    // Distancia m�nima para que el gato se detenga
    public float stopDistance = 0.1f;

    // Umbral de movimiento del personaje para considerar que se est� moviendo
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

        // Guardamos la posici�n inicial del personaje
        lastTargetPosition = target.position;
    }

    void Update()
    {
        // Calcula la posici�n deseada usando el offset relativo a la orientaci�n del personaje
        Vector3 desiredPos = target.position + target.TransformDirection(offset);
        float distance = Vector3.Distance(transform.position, desiredPos);

        // Mueve al gato si est� lejos del punto deseado
        if (distance > stopDistance)
        {
            agent.SetDestination(desiredPos);
        }
        else
        {
            agent.ResetPath();
        }

        // Calcula cu�nto se ha movido el personaje desde el �ltimo frame
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

        // Actualizamos la posici�n anterior del personaje para la siguiente comparaci�n
        lastTargetPosition = target.position;
    }
}
