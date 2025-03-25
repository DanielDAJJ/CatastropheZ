using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombiesPooling : MonoBehaviour
{

    public static ZombiesPooling Instance { get; private set;}

    [SerializeField] GameObject prefabZombie;

    [SerializeField] List<GameObject> zombieList = new();

    void Awake()
    {
        if(Instance==null){
         Instance=this;
        }

        else
        {
           Destroy(this.gameObject); 
        }
    }

    void Start()
    {
        MakePool(50);
    }

    void MakePool(int poolSize)
    {   
        for (int i = 0; i < poolSize; i++)
        {
            GameObject zombie;
            zombie= Instantiate(prefabZombie);
            //zombie.SetActive(false);
            zombieList.Add(zombie);
            zombie.transform.SetParent(transform);
        }

    }

    public GameObject useZombie()
    {   
        foreach (GameObject zombie in zombieList)
        {
            if (!zombie.activeInHierarchy)
            {
                return zombie;
            }            
        }
        MakePool(1);
        return zombieList.Last();
    }
    
}