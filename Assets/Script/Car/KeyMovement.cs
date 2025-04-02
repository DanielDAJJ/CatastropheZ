using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    // Start is called before the first frame update    
    [SerializeField] float velRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * velRotation * Vector3.up);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
