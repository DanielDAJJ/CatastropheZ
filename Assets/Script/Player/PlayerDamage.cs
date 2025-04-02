using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] Vector3 originalPosition;  
    [SerializeField] bool isShaking;  
    [SerializeField] bool firstHit;  
    [SerializeField] float shakeDuration=2f;  
    [SerializeField] float shakeAmount=2f;  
    [SerializeField] ParticleSystem bloodSplashEffect;  
    [SerializeField] Death death;  

     void Start()
    {
        firstHit=true;
        death=GetComponent<Death>();       
    }

   
    public void PlayerReceiveDamage()
    {   
        if(firstHit)
        {   
            death.TakeDamage(1);
            bloodSplashEffect.Play();
            firstHit=false;
            StartCoroutine(InvincibleTime());
           
            
        }
    }


    private IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(3);
        firstHit=true;
        bloodSplashEffect.Stop();
    }

   
}
