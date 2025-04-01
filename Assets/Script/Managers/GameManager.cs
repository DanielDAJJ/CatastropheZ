using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }
    public void GameWin()
    {

    }

    public void GamePause()
    {

    }

}
