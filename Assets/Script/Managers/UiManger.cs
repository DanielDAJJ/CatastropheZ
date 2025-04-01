using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManger : MonoBehaviour
{
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour cameraController;
    public GameObject mainMenuUi;
    void Start()
    {
        ShowMenu();
    }

    void Update()
    {
        
    }
    public void ShowMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }
        mainMenuUi.SetActive(true);
    }
    public void StartGame()
    {
        if (mainMenuUi != null)
        {
            mainMenuUi.SetActive(false);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }
        mainMenuUi.SetActive(false);
    }
}
