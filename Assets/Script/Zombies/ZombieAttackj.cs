using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackj : MonoBehaviour
{   
    public PlayerDamage playerDamage;
    public float radius = 0.14f;
    public LayerMask layerMask; 
  
   [SerializeField] float _radiusRangeAttack;

     public Animator animator; // Referencia al Animator
    public string animationName = "Attack"; // Nombre de la animación que deseas comprobar

    public GameObject player;
    public float pushForce = 10f; // Fuerza del empujón


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {       
        // Obtener el estado de la animación actual (en el primer layer, normalmente el 0)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        
        if (stateInfo.IsName(animationName))
        {
            bool isColliding = Physics.CheckSphere(transform.position, radius, layerMask);
            if(isColliding)
            {   
               playerDamage.PlayerReceiveDamage();
            
            }
        }
        
    }

       private void OnDrawGizmos()
     {
   
    
        // Calculamos la posición de la esfera para la comprobación
        Vector3 spherePosition = transform.position;
        
        // Establecemos el color de la esfera que se va a dibujar en el editor (rojo en este caso)
        Gizmos.color = Color.red;

        // Dibujamos la esfera en la posición calculada con el radio del controlador (lo mismo que se usa en CheckSphere)
        Gizmos.DrawSphere(spherePosition, _radiusRangeAttack);
        }
}
