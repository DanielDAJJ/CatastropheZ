using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackj : MonoBehaviour
{   
    public PlayerDamage playerDamage;
    public float radius = 0.14f;
    public LayerMask layerMask; 
    public LayerMask carMask; 
    [SerializeField] CarController carController;
    [SerializeField] bool firstHitCar;
    
    [SerializeField] float _radiusRangeAttack;

    public Animator animator; // Referencia al Animator
    public string animationName = "Attack"; // Nombre de la animación que deseas comprobar



    void Start()
    {
        playerDamage=GameObject.Find("Female Player").GetComponent<PlayerDamage>();
        Transform rootTransform = transform;

        while (rootTransform.parent != null)
        {
            rootTransform = rootTransform.parent;
        }

        GameObject rootObject = rootTransform.gameObject;
        animator=rootObject.GetComponent<Animator>();
        carController= GameObject.Find("Monster Car").GetComponent<CarController>();
        carMask= LayerMask.GetMask("Monster Car");
        firstHitCar=true;
    }

    // Update is called once per frame
    void Update()
    {       
    
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        
        if (stateInfo.IsName(animationName))
        {
            bool isColliding = Physics.CheckSphere(transform.position, radius, layerMask);
            if(isColliding)
            {   
               playerDamage.PlayerReceiveDamage();  
            
          
            }

            bool isCollidingCar = Physics.CheckSphere(transform.position, radius, carMask);
            if(isCollidingCar && firstHitCar)
            {   
                carController.CarDamage();  
                firstHitCar=false;
                StartCoroutine(FirstHitTimer());

            }


        }
    }

    private IEnumerator FirstHitTimer()
    {
        yield return  new WaitForSeconds(2); 
        firstHitCar=true;
    }
        
    private void OnDrawGizmos()
    {
    // Establece el color del gizmo a rojo
    Gizmos.color = Color.red;

    // Dibuja una esfera en la posición del objeto con un radio de 1
    Gizmos.DrawSphere(transform.position,radius); // Puedes cambiar 1f por el radio que desees
    }
   }
