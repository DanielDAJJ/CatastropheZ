
using UnityEngine;

public class PlayerController : MonoBehaviour
{
 private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    public bool isDriving;
    [SerializeField] bool isNearCar;

    private void Start()
    {
        isNearCar=false;
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.E) && isNearCar)
        {
            isDriving=!isDriving;
        }
        if(!isDriving)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Makes the player jump
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
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


