using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZobiesRigidbodyController : MonoBehaviour
{
    public PlayerEnableCar playerEnableCar;
    Rigidbody zombieRB;
    NavMeshAgent zombieNav;
    // Start is called before the first frame update
    void Start()
    {
        zombieRB= GetComponent<Rigidbody>();
        zombieNav= GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (playerEnableCar.isDriving)
        {
            zombieRB.isKinematic=false;
            zombieNav.enabled=false;
        }
        else
        {
            zombieRB.isKinematic=true;
          
        }
    }
}
