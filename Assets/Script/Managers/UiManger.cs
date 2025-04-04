using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManger : MonoBehaviour
{
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour cameraController;
    public GameObject mainMenuUi;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public GameObject CamaraInstance;
    public string gameSceneName = "CityIntegracion";
    public string menuSceneName = "CityDany";
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
        CloseAllPanels();
    }
    public void StartGame()
    {
        if (mainMenuUi != null)
        {
            mainMenuUi.SetActive(false);
        }
        if (CamaraInstance != null)
        {
            CamaraInstance.SetActive(false);
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
        SceneManager.LoadScene(gameSceneName);
        mainMenuUi.SetActive(false);
    }
    public void OpenOptions()
    {
        CloseAllPanels();
        mainMenuUi.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void OpenControls()
    {
        CloseAllPanels();
        mainMenuUi.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void OpenCredits()
    {
        CloseAllPanels();
        mainMenuUi.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void CloseAllPanels()
    {
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Estoy cerrando");
    }
}
