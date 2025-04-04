using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{

    public IdleState startingState;

    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public Transform currentTarget;
    public float distanceFromCurrentTarget;


    [Header("Animator")]
    public Animator animator;


    [Header("NavMesh Agent")]
    public NavMeshAgent zombieNavmeshAgent;

    [Header("Rigidbody")]
    public Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5f;

    [Header("Attack")]
    public float attackCoolDownTimer;
    public float minimumAttackDistance = 1f;

    public bool hitCh;
    public bool zombieStunt;


    private void Awake()
    {
        currentState = startingState;
        animator = GetComponent<Animator>();
        zombieNavmeshAgent = GetComponent<NavMeshAgent>();
        zombieRigidbody = GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
        
    }


    private void Update()
    {
      //  zombieNavmeshAgent.transform.localPosition = Vector3.zero;

        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer = attackCoolDownTimer - Time.deltaTime;
        }

        if (currentTarget != null)
        {
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.position, transform.position);
        }
    }

    private void HandleStateMachine()
    {
        State nextState;

        if (currentState != null)
        {
            nextState = currentState.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }
        }


    }

    public void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.CompareTag("Chancleta") && !zombieStunt)
        {      
          print("Chancletazo");
          StartCoroutine(ZombieStunt());
          hitCh=true;
          zombieStunt=true;
                          
        }

    }

    private IEnumerator ZombieStunt()
    {
        yield return new WaitForSeconds(3);
        zombieStunt=false;
    }

}
