using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] Vector3 originalPosition;  
    [SerializeField] bool isShaking;  
    [SerializeField] bool firstHit;  
    [SerializeField] float shakeDuration=2f;  
    [SerializeField] float shakeAmount=2f;  
     void Start()
    {
        firstHit=true;        
    }

   
    public void PlayerReceiveDamage()
    {   
        if(firstHit)
        {   
            print("Player recibe daño");
            firstHit=false;
            StartCoroutine(InvincibleTime());
           
            
        }
    }


    private IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(3);
        firstHit=true;
    }

   
}
