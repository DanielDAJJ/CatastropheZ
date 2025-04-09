using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenuManger : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Opcional - Script de movimiento del jugador")]
    [SerializeField] private MonoBehaviour playerMovementScript;

    [SerializeField] private bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        Debug.Log("Update activo. Time.timeScale = " + Time.timeScale);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Tecla Escape presionada");
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Debug.Log("Juego Pausado");
        Time.timeScale = 0f;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Debug.Log("Juego Reanudado");
        Time.timeScale = 1f;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
