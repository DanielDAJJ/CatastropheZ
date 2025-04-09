using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    void Awake()
    {
        if(Instance==null)
        {
           Instance=this;
           DontDestroyOnLoad(this.gameObject);
        }
        else
        {
           Destroy(this.gameObject);
        }
        
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {

    }
    public void GameOver()
    {
        print("Perdiste");
    }
    public void GameWin()
    {
        SceneManager.LoadScene("Win_2");
    }

    public void GamePause()
    {

    }

}
