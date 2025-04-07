using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaGameOver : MonoBehaviour
{
    public GameObject deathPrefab;
    public Transform camaraGameOver;
    public float rotationSpeed = 20f;
    public float radius = 5f;
    private float angle = 0;
    void Start()
    { 
       if(deathPrefab == null || camaraGameOver == null)
        {
            Debug.Log("Faltan referencias en CamaraGameOver");
            return;
        }
        camaraGameOver.position = deathPrefab.transform.position + new Vector3(radius, 0, 0);
        camaraGameOver.LookAt(deathPrefab.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamara();
    }
    void RotateCamara()
    {
        if (deathPrefab == null || camaraGameOver == null) return;
        angle += rotationSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius - 10f;
        camaraGameOver.position = deathPrefab.transform.position + new Vector3(x, 10f, z);
        camaraGameOver.LookAt(deathPrefab.transform.position);
    }
}
