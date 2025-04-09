using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceUi : MonoBehaviour
{
    private static PersistenceUi instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CityDany") // Asegúrate de usar el nombre correcto
        {
            gameObject.SetActive(true); // Desactivas la UI del juego
        }
        else
        {
            gameObject.SetActive(false); // Activas la UI cuando estás en el gameplay
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
