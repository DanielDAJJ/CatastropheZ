using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    public bool isInCar;

    public int carLife;
    
    [SerializeField] PlayerEnableCar player;
    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;
    [SerializeField] private Transform newPlayerTransform; 
    [SerializeField] private GameObject cinemachine;
    [SerializeField] private GameObject ligth;
    Rigidbody rbCar;

    [SerializeField] ParticleSystem particleFire;
    [SerializeField] ParticleSystem particleSmoke;
    [SerializeField] GameObject carExplode;

    public float carVelocity;
   

    void Start()
    {   
        carLife=50;
        isInCar=false;
        rbCar=gameObject.GetComponent<Rigidbody>();
        ligth=transform.Find("Ligth").gameObject;
    }

    void Update()
    {

       if (Input.GetKeyDown(KeyCode.E) && isInCar)
        {
            player.isDriving=false;
            ActivatePlayer();
            
        }

        carVelocity = Vector3.Magnitude(rbCar.velocity);

        if(isInCar)
        {
            cinemachine.SetActive(true);
            rbCar.isKinematic=false;
            ligth.SetActive(true);
            player.transform.position=new Vector3(0f,1000f,0f);
        }
        else
        {
            cinemachine.SetActive(false);
            rbCar.isKinematic=true;
            ligth.SetActive(false);
                       
        }
       
    }

    void FixedUpdate()
    {
        if(player.isDriving){
            isInCar=true;
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            DeactivatePlayer();
        }
        else
        {
          currentbreakForce= breakForce;
           ApplyBreaking(); 

        }

  
    }

    private void ActivatePlayer()
    {   
        isInCar=false;
        player.gameObject.SetActive(true);
        player.gameObject.GetComponent<CharacterController>().enabled=false;
        player.transform.SetPositionAndRotation(newPlayerTransform.position, newPlayerTransform.rotation);
        player.gameObject.GetComponent<CharacterController>().enabled=true;
    }

    private void DeactivatePlayer()
    {
        player.gameObject.SetActive(false);
    }

    private void GetInput()
    {
       horizontalInput= Input.GetAxis("Horizontal");
       verticalInput= Input.GetAxis("Vertical");
       isBreaking= Input.GetKey(KeyCode.Space); 
    }
    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque=verticalInput*motorForce;
        frontRightWheelCollider.motorTorque=verticalInput*motorForce;

        rearLeftWheelCollider.motorTorque=verticalInput*motorForce;
        rearRightWheelCollider.motorTorque=verticalInput*motorForce;
        currentbreakForce= isBreaking ? breakForce :0f;
        ApplyBreaking();

    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
      
    }
        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) 
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.SetPositionAndRotation(pos, rot);
    }
   

    public void CarDamage()
    {
      carLife--;
      if (carLife==25)
      {
        particleSmoke.Play();
      } 

      if (carLife==0)
      {
        particleFire.Play();
        gameObject.GetComponent<CarController>().enabled=false;
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 1; i < 21; i++)
        {
            children[i].gameObject.SetActive(false);    
        }

        carExplode.SetActive(true);
        gameObject.GetComponent<MeshFilter>().mesh=null;
      }  

    }


}
