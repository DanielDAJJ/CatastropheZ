using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public float health; // Vida inicial
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontr� el componente Animator en " + gameObject.name);
        }
    }

    // M�todo para aplicar da�o
    public void TakeDamage(float damage)
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
    }
}
