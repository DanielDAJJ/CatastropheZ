using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailRotor : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationSpeed = 800f; // Velocidad de giro en grados por segundo

    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
