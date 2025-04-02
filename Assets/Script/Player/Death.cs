using System.Collections;
using System.Collections.Generic;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

public class Death : MonoBehaviour
{
    public int health; 
    private Animator animator;
    private bool isDead = false;
    
    private CharacterController characterController;
    private InputReader inputReader;

    void Start()
    {   
        characterController= GetComponent<CharacterController>();
        inputReader= GetComponent<InputReader>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontr� el componente Animator en " + gameObject.name);
        }
    }

    // M�todo para aplicar da�o
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        health -= damage;
        Debug.Log("Vida actual: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    // M�todo que activa la animaci�n de muerte
    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("�Te moriste! Pero cada ca�da es una lecci�n para levantarte m�s fuerte.");

        // Activa la animaci�n de muerte usando el par�metro bool "isDeath"
        animator.SetBool("isDeath", true);

        // Aqu� podr�as agregar l�gica extra, como deshabilitar el movimiento o reiniciar el nivel.
        characterController.enabled=false;
        inputReader.enabled=false;

    }
}
