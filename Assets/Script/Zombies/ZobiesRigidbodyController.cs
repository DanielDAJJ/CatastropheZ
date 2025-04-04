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
        playerEnableCar= GameObject.Find("Female Player").GetComponent<PlayerEnableCar>();

    }

    // Update is called once per frame
    void Update()
    {   
        if (playerEnableCar.isDriving)
        {
            zombieRB.isKinematic=false;
        }
        else
        {
            zombieRB.isKinematic=true;
          
        }
    }
}
