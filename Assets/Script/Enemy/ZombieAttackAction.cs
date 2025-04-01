using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " A.I/Action/Zombie Attack Action")]
public class ZombieAttackAction : ScriptableObject
{
    [Header("Attack Animation")]
    public string attackAnimation;

    [Header("Attack Cooldown")]
    public float attackCooldown = 5f;

    [Header("Attack Angles & Distances")]
    public float maximunAttackAngle;                 //El �ngulo m�ximo de visi�n necesario para realizar ESTE ataque
    public float minimunAttackAngle;                 //El �ngulo m�nimo de visi�n necesario para realizar ESTE ataque
    public float minimunAttackDistance = 1f;              //La distancia m�nima desde el objetivo actual necesaria para realizar ESTE ataque
    public float maximunAttackDistance = 3.5f;              //La distancia m�xima desde el objetivo actual necesaria para realizar ESTE ataque
}
