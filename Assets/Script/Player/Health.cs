using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float healDuration = 6f;     // Duración total de la animación de curación
    [SerializeField] private float particleDelay = 2f;      // Retraso antes de reproducir la partícula
    [SerializeField] private float particleDuration = 2f;   // Tiempo que la partícula estará activa
    [SerializeField] private ParticleSystem healParticle;   // Asigna aquí tu sistema de partículas

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetBool("Healing", true);
            StartCoroutine(HealingSequence());
        }
    }

    private IEnumerator HealingSequence()
    {
        // Espera para iniciar la partícula
        yield return new WaitForSeconds(particleDelay);
        healParticle.Play();

        // Espera mientras la partícula está activa y luego la detiene
        yield return new WaitForSeconds(particleDuration);
        healParticle.Stop();

        // Espera el tiempo restante de la animación
        yield return new WaitForSeconds(healDuration - particleDelay - particleDuration);
        anim.SetBool("Healing", false);
    }
}
