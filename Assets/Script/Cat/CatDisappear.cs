using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDisappear : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            InventoryUIManager.Instance.AddCat();
            Destroy(this.gameObject);
        }
    }

}
