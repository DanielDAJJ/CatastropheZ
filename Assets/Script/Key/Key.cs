using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Rotación")]
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);

    [Header("Flotación")]
    public float floatAmplitude = 0.5f; // Qué tan alto/flota
    public float floatFrequency = 1f;   // Qué tan rápido sube/baja

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotar el objeto
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // Movimiento vertical (flotación)
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}
