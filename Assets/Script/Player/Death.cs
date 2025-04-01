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
            Debug.LogError("No se encontró el componente Animator en " + gameObject.name);
        }
    }

    // Método para aplicar daño
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

    // Método que activa la animación de muerte
    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("¡Te moriste! Pero cada caída es una lección para levantarte más fuerte.");

        // Activa la animación de muerte usando el parámetro bool "isDeath"
        animator.SetBool("isDeath", true);

        // Aquí podrías agregar lógica extra, como deshabilitar el movimiento o reiniciar el nivel.
    }
}
