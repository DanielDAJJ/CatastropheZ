using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuitTargetState : State
{

    AttackState AttackState;
    IdleState IdleState;
    [SerializeField] CarController carController;
    public bool persuitCar=false;



    private void Awake()
    {
        AttackState = GetComponent<AttackState>();
        IdleState = GetComponent<IdleState>();
    }


    public override State Tick(ZombieManager zombieManager)
    {
     //   Debug.Log("Running Persuiting target");
        MoveTowardsCurrentTarget(zombieManager);
        RotateTowardsTarget(zombieManager);
        

        if (zombieManager.distanceFromCurrentTarget <= zombieManager.minimumAttackDistance)
        {
            return AttackState;
        }

        else if(zombieManager.distanceFromCurrentTarget>15f)
        {   
        
            zombieManager.currentTarget=null;
            zombieManager.zombieNavmeshAgent.enabled = false;
            StopPersuitAnimation(zombieManager);
            return IdleState;
        }
               
            
        else
        {   
            return this;
        }


        
    }
   

    private void MoveTowardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

    }

    private void RotateTowardsTarget(ZombieManager zombieManager)
    {
        zombieManager.zombieNavmeshAgent.enabled = true;
        zombieManager.zombieNavmeshAgent.SetDestination(zombieManager.currentTarget.transform.position);
        zombieManager.transform.rotation = Quaternion.Slerp(zombieManager.transform.rotation, zombieManager.zombieNavmeshAgent.transform.rotation,zombieManager.rotationSpeed / Time.deltaTime);
    }

    private void StopPersuitAnimation(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical",0);
    }
}
