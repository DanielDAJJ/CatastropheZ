using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopteroRotor : MonoBehaviour
{
    public float rotationSpeed = 500f; // Velocidad de giro en grados por segundo

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
