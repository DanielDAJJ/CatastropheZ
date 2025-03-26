using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class AttackState : State
{
    [Header("Zombie Attacks")]
    public ZombieAttackAction[] zombieAttackActions;

    [Header("Potential Attacks Performable Right Now")]
    public List<ZombieAttackAction> potentialAttacks;


    [Header("Current Attack Being Performed")]
    public ZombieAttackAction currentAttack;


    [Header("State Flags")]
    public bool hasPerformedAttack;



    public override State Tick(ZombieManager zombieManager)
    {
        Debug.Log("Attack");
        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
        

        if (!hasPerformedAttack && zombieManager.attackCoolDownTimer <= 0)
        {


        }

        return this;
    }




}

