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
    private float attackAnimationLength = 2.633f;
    [SerializeField]private bool attackFinished = false;

    public Animator animator; // Referencia al Animator
    public string animationName = "Attack"; // Nombre de la animación que deseas comprobar

    public PlayerEnableCar playerEnableCar;



    public override State Tick(ZombieManager zombieManager)
    {
      //  Debug.Log("Attack");

        // Detenemos el movimiento en el Blend Tree, para que no se mezcle con locomoci�n.
        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
        zombieManager.zombieNavmeshAgent.enabled = false;;


        // Si no hemos disparado el ataque y el cooldown permite atacar.
        if (!hasPerformedAttack && zombieManager.attackCoolDownTimer <= 0)
        {
            // Disparamos el trigger para cambiar al estado "Attack" en el Animator.
            zombieManager.animator.SetTrigger("Attack");
            attackFinished=false;   

       
            // Reiniciamos el cooldown para evitar ataques continuos.
            zombieManager.attackCoolDownTimer = 2f; // Ajusta este valor seg�n necesites.
            hasPerformedAttack = true;

            StartCoroutine(WaitForAttackAnimation(zombieManager));
        }

        // Si el objetivo se aleja un poco, pasamos a otro estado (por ejemplo, Chase o Idle)
        if (zombieManager.currentTarget != null)
        {
              AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            float dist = Vector3.Distance(zombieManager.currentTarget.position, zombieManager.transform.position);
            // Si el jugador se aleja m�s all� de una tolerancia, volvemos a locomoci�n.
            if (dist > zombieManager.minimumAttackDistance + 0.5f && !stateInfo.IsName(animationName))
            {
                hasPerformedAttack = false; // Reseteamos para futuros ataques.
               
                return zombieManager.startingState; // O retorna el ChaseState si lo tienes.
            }
        }

        // Nos quedamos en AttackState hasta que se de una condici�n para cambiar.
        return this;
    }

    private IEnumerator WaitForAttackAnimation(ZombieManager zombieManager)
    {
        yield return new WaitForSeconds(attackAnimationLength);

        hasPerformedAttack=false;
        attackFinished=true;
    }




}

