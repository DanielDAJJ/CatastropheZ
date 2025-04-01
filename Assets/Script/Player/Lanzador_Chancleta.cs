using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzador_Chancleta : MonoBehaviour
{
    public Transform handSpawnPoint; // Objeto vacío en la mano del personaje
    public GameObject slipperPrefab; // Prefab de la chancleta
    public float throwForce = 10f; // Fuerza del lanzamiento
    public float torqueForce = 10f; // Fuerza del giro
    public Camera playerCamera; // Cámara del jugador (arrástrala desde el Inspector)

    public void ThrowSlipper()
    {
        // Instancia la chancleta en la posición y rotación del socket.
        GameObject slipper = Instantiate(slipperPrefab, handSpawnPoint.position, handSpawnPoint.rotation);

        // Si la chancleta tiene Rigidbody, se le aplica la fuerza y el torque.
        Rigidbody rb = slipper.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Obtiene la dirección hacia donde mira la cámara
            Vector3 throwDirection = playerCamera.transform.forward;
            throwDirection.y = 0; // Opcional: Evita que la chancleta suba o baje demasiado

            // Lanza la chancleta en la dirección de la cámara
            rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);

            // Aplica un torque para que rote la chancleta
            rb.AddTorque(handSpawnPoint.up * torqueForce, ForceMode.Impulse);
        }

        // Destruye la chancleta a los 8 segundos
        Destroy(slipper, 8f);
    }
}
