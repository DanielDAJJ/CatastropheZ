using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    
    public virtual State Tick(ZombieManager zombieManager)
    {
        Debug.Log("Running state");
        return this;
    }
}
