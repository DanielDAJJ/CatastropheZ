using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzador_Chancleta : MonoBehaviour
{
    public Transform handSpawnPoint; // Objeto vac�o en la mano del personaje
    public GameObject slipperPrefab; // Prefab de la chancleta
    public float throwForce = 10f; // Fuerza del lanzamiento
    public float torqueForce = 10f; // Fuerza del giro
    public Camera playerCamera; // C�mara del jugador (arr�strala desde el Inspector)

    public void ThrowSlipper()
    {
        // Instancia la chancleta en la posici�n y rotaci�n del socket.
        GameObject slipper = Instantiate(slipperPrefab, handSpawnPoint.position, handSpawnPoint.rotation);

        // Si la chancleta tiene Rigidbody, se le aplica la fuerza y el torque.
        Rigidbody rb = slipper.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Obtiene la direcci�n hacia donde mira la c�mara
            Vector3 throwDirection = playerCamera.transform.forward;
            throwDirection.y = 0; // Opcional: Evita que la chancleta suba o baje demasiado

            // Lanza la chancleta en la direcci�n de la c�mara
            rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);

            // Aplica un torque para que rote la chancleta
            rb.AddTorque(handSpawnPoint.up * torqueForce, ForceMode.Impulse);
        }

        // Destruye la chancleta a los 8 segundos
        Destroy(slipper, 8f);
    }
}
