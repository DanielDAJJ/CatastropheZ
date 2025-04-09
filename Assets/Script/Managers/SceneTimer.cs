using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTimer : MonoBehaviour
{
    [SerializeField] private float tiempoParaCambiar = 75f; // 1 minuto 15 segundos

    void Start()
    {
        StartCoroutine(CambiarEscenaDespuesDeTiempo());
    }

    IEnumerator CambiarEscenaDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoParaCambiar);
        SceneManager.LoadScene("PostCredits");
    }
}
