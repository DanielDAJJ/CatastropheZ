using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceUi : MonoBehaviour
{
    private static PersistenceUi instance;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
