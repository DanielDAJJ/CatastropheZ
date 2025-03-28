using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{
    public Transform[] interestPoints;
    public float moveSpeed = 2f;
    private bool isMoving = false;
    public Camera targetCamera;

    void Start()
    {
        if (targetCamera == null)
        {
            Debug.LogError("No se encontró ninguna cámara en el objeto con MenuCameraController.");
        }
    }
    void Update()
    {
        
    }
    public void MoveToPoint(int index)
    {
        if (isMoving || targetCamera == null || index < 0 || index >= interestPoints.Length) return;

        Vector3 targetPos = interestPoints[index].position;
        Quaternion targetRot = interestPoints[index].rotation;

        Debug.Log($"Moviendo cámara a punto {index}: {targetPos}");
        StartCoroutine(MoveCamera(targetPos, targetRot));
    }
    private IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
        isMoving = true;
        Vector3 startPosition = targetCamera.transform.position;
        Quaternion startRotation = targetCamera.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            float t = elapsedTime / 1f; // Normaliza el tiempo de interpolación
            targetCamera.transform.position = Vector3.Lerp(startPosition, targetPos, t);
            targetCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRot, t);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        targetCamera.transform.position = targetPos;
        targetCamera.transform.rotation = targetRot;
        isMoving = false;

        Debug.Log($"Movimiento completado. Nueva posición: {targetCamera.transform.position}");
    }
}
