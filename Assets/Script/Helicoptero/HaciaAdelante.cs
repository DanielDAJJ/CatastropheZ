using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HaciaAdelante : MonoBehaviour
{
    public float speed; // Velocidad del objeto
    private bool canMove = false;


    void Start()
    {
        Destroy(gameObject, 25f); // Destruye el objeto después de 20 segundos
        StartCoroutine(StartMovementAfterDelay(5f));
    }

    void Update()
    {
        if (canMove)
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }

    IEnumerator StartMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true; // Activa el movimiento después del delay
    }

}
