using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthStatus : MonoBehaviour
{
    public GameObject estadoMonitor;
    private Image estadoImage;
    // Start is called before the first frame update
    void Awake()
    {
        if (estadoMonitor != null)
        {
            estadoImage = estadoMonitor.GetComponent<Image>();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CambiarColorMonitor(Color nuevoColor)
    {
        if (estadoImage != null)
        {
            estadoImage.color = nuevoColor;
        }
    }
}
