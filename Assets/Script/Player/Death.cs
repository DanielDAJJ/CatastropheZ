using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Synty.AnimationBaseLocomotion.Samples;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Death : MonoBehaviour
{
    public int health; 
    private Animator animator;
    private bool isDead = false;
    
    private CharacterController characterController;
    private InputReader inputReader;
    private SamplePlayerAnimationController samplePlayerAnimationController;
    private HealthStatus playerStatus;
    public string gameOverSceneName = "DeathScene";

    void Start()
    {   
        characterController= GetComponent<CharacterController>();
        inputReader= GetComponent<InputReader>();
        animator = GetComponent<Animator>();
        samplePlayerAnimationController=GetComponent<SamplePlayerAnimationController>();
        playerStatus = GetComponent<HealthStatus>();
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
        if (health >= 3)
        {
            playerStatus.CambiarColorMonitor(Color.green);
        }
        else if (health == 2)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.i);
            playerStatus.CambiarColorMonitor(Color.yellow);
        }
        else if (health == 1)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.h);
            playerStatus.CambiarColorMonitor(Color.red);
        }
        if (health <= 0)
        {
            Die();
        }
    }
    public void TakeHealth()
    {
        health++;
         if (health == 3)
        {
            playerStatus.CambiarColorMonitor(Color.green);
        }
        else if (health == 2)
        {
            playerStatus.CambiarColorMonitor(Color.yellow);
        }
    }

   
    void Die()
    {
        if (isDead)
            return;

        isDead = true;
       
       // Activa la animaci�n de muerte usando el par�metro bool "isDeath"
        animator.SetBool("isDeath", true);

        // Aqu� podr�as agregar l�gica extra, como deshabilitar el movimiento o reiniciar el nivel.
        characterController.enabled=false;
        inputReader.enabled=false;
        samplePlayerAnimationController.enabled=false;
        GameManager.Instance.GameOver(); 
        StartCoroutine(CargarEscenaGameOver());
    }

     private IEnumerator CargarEscenaGameOver()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(gameOverSceneName);
    }

  
}
