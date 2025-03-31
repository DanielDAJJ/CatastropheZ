
using UnityEngine;

public class PlayerEnableCar : MonoBehaviour
{
    public bool isDriving;
    public bool hasKey;
    [SerializeField] bool isNearCar;

    private void Start()
    {
        isNearCar=false;
        hasKey=false;
    
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.E) && isNearCar && hasKey)
        {
            isDriving=!isDriving;
        }
        

    }

    void OnTriggerEnter(Collider other)
    {   
        if(other.CompareTag("Car")) 
        {
            isNearCar=true;
        }
         if(other.CompareTag("Key")) 
        {
            hasKey=true;

        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Car")) 
        {
            isNearCar=false;
        }
    }
}


