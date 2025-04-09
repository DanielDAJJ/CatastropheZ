using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Pause : MonoBehaviour
{
    public GameObject ObjetoMenuPause;
    public bool Pause = false;
    public GameObject MenuSalir;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Pause == false)
            {               
                ObjetoMenuPause.SetActive(true);
                Pause = true;
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
            else if(Pause == true)
            {
               Resumir();
            }
        }
        
    }

    public void Resumir()
    {        
        ObjetoMenuPause.SetActive(false);
        MenuSalir.SetActive(false);
        Pause = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MenuPrincipal(string NombreMenu)
    {
        SceneManager.LoadScene(NombreMenu);
    }


    public void SalirJuego()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
