using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverOptions : MonoBehaviour
{
    [Header("Scene Names")]
    public string gameSceneName = "CityIntegracion";
    public string menuSceneName = "CityDany";
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Estoy cerrando");
    }
    public void ReturnToMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(menuSceneName);
    }
    public void RestartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(gameSceneName);
    }
}
