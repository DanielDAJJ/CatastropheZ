
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
        if (Input.GetKeyDown(KeyCode.E) && isNearCar && hasKey && InventoryUIManager.Instance.catCount==3)
        {
            isDriving=!isDriving;
        }
        else if (Input.GetKeyDown(KeyCode.E) && isNearCar && (!hasKey || InventoryUIManager.Instance.catCount!=3))
        {
         print("Te falta la llave o los 3 michis");
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


