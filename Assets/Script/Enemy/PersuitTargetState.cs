using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuitTargetState : State
{

    AttackState AttackState;
    IdleState IdleState;
    [SerializeField] CarController carController;
    public bool persuitCar=false;
    public string animationName = "Attack";



    private void Awake()
    {
        AttackState = GetComponent<AttackState>();
        IdleState = GetComponent<IdleState>();
        carController = GameObject.Find("Monster Car").GetComponent<CarController>();
    }


    public override State Tick(ZombieManager zombieManager)
    {
        AnimatorStateInfo stateInfo = zombieManager.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationName))
        {
            zombieManager.animator.SetFloat("Vertical", 1);
        }
        
   
        MoveTowardsCurrentTarget(zombieManager);
        RotateTowardsTarget(zombieManager);
        

        if (zombieManager.currentTarget.name=="Female Player" && zombieManager.distanceFromCurrentTarget <= zombieManager.minimumAttackDistance)
        {
            return AttackState;
        }
        else if(zombieManager.currentTarget.name=="Monster Car" &&  zombieManager.nearCar)
        {
            return AttackState;
        }

        else if((zombieManager.currentTarget.name=="Female Player") &&(zombieManager.distanceFromCurrentTarget>15f || zombieManager.hitCh))
        {   
            zombieManager.currentTarget=null;
            zombieManager.zombieNavmeshAgent.enabled = false;
            StopPersuitAnimation(zombieManager);
            zombieManager.hitCh=false;
            return IdleState;
        }

        else if(zombieManager.currentTarget.name=="Monster Car" && !carController.isInCar)
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
