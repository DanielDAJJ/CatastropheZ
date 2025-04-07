using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlbertaMovement : MonoBehaviour
{   
   
    [SerializeField] Transform finalPosition;
    [SerializeField] GameObject heli;
     private NavMeshAgent navMesh;   
     private bool walkEnable;
     Animator animator;
    void Start()
    {   
        navMesh=GetComponent<NavMeshAgent>();
        StartCoroutine(WaitToStart());
        animator=GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {  
        if(walkEnable)
        {   
            animator.SetBool("walk",true);
            MoveToHeli();
            if(transform.position.x==finalPosition.position.x && transform.position.z==finalPosition.position.z)
                {
                    heli.GetComponent<UpAndGo>().enabled=true;
                    transform.gameObject.SetActive(false);
                }   
        }              
    }

    private void MoveToHeli()
    {
        navMesh.SetDestination(finalPosition.position);
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2);
        walkEnable=true;

    }


}
