using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float healDuration = 6f;     // Duraci�n total de la animaci�n de curaci�n
    [SerializeField] private float particleDelay = 2f;      // Retraso antes de reproducir la part�cula
    [SerializeField] private float particleDuration = 2f;   // Tiempo que la part�cula estar� activa
    [SerializeField] private ParticleSystem healParticle;   // Asigna aqu� tu sistema de part�culas
    [SerializeField] Death death; 


    void Start()
    {
        anim = GetComponent<Animator>();
        death=GetComponent<Death>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && InventoryUIManager.Instance.healIcon.activeSelf && death.health<3)
        { 

            anim.SetBool("Healing", true);
            StartCoroutine(HealingSequence());
            death.TakeHealth();
            InventoryUIManager.Instance.UseHeal();
            
        }
    }

    private IEnumerator HealingSequence()
    {
        // Espera para iniciar la part�cula
        yield return new WaitForSeconds(particleDelay);
        healParticle.Play();

        // Espera mientras la part�cula est� activa y luego la detiene
        yield return new WaitForSeconds(particleDuration);
        healParticle.Stop();

        // Espera el tiempo restante de la animaci�n
        yield return new WaitForSeconds(healDuration - particleDelay - particleDuration);
        anim.SetBool("Healing", false);
    }
}
