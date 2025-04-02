using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPersistence : MonoBehaviour
{
    private static ManagerPersistence instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
