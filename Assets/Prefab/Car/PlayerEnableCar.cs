
using UnityEngine;

public class PlayerEnableCar : MonoBehaviour
{
    public bool isDriving;
    [SerializeField] bool isNearCar;

    private void Start()
    {
        isNearCar=false;
    
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.E) && isNearCar)
        {
            isDriving=!isDriving;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        isNearCar=true;

    }

    void OnTriggerExit(Collider other)
    {
        isNearCar=false;
        
    }
}


