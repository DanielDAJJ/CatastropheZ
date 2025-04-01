using Cinemachine;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
   [SerializeField] CinemachineVirtualCamera cameraCar;
   [SerializeField] CinemachineVirtualCamera cameraPlayer;
   [SerializeField] PlayerEnableCar player;
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isDriving)
        {
           cameraCar.GetComponent<CinemachineVirtualCamera>().Priority=10;
           cameraPlayer.GetComponent<CinemachineVirtualCamera>().Priority=0;
    
        }
        else
        {
        cameraCar.GetComponent<CinemachineVirtualCamera>().Priority=0;
        cameraPlayer.GetComponent<CinemachineVirtualCamera>().Priority=10;

        }

    }
}
