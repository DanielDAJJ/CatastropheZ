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
      //  Debug.Log("Attack");

        // Detenemos el movimiento en el Blend Tree, para que no se mezcle con locomoci�n.
        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);

        // Si no hemos disparado el ataque y el cooldown permite atacar.
        if (!hasPerformedAttack && zombieManager.attackCoolDownTimer <= 0)
        {
            // Disparamos el trigger para cambiar al estado "Attack" en el Animator.
            zombieManager.animator.SetTrigger("Attack");

            // Reiniciamos el cooldown para evitar ataques continuos.
            zombieManager.attackCoolDownTimer = 2f; // Ajusta este valor seg�n necesites.
            hasPerformedAttack = true;
        }

        // Si el objetivo se aleja un poco, pasamos a otro estado (por ejemplo, Chase o Idle)
        if (zombieManager.currentTarget != null)
        {
            float dist = Vector3.Distance(zombieManager.currentTarget.position, zombieManager.transform.position);
            // Si el jugador se aleja m�s all� de una tolerancia, volvemos a locomoci�n.
            if (dist > zombieManager.minimumAttackDistance + 0.5f)
            {
                hasPerformedAttack = false; // Reseteamos para futuros ataques.
                return zombieManager.startingState; // O retorna el ChaseState si lo tienes.
            }
        }

        // Nos quedamos en AttackState hasta que se de una condici�n para cambiar.
        return this;
    }




}

