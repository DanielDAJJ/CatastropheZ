using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    public GameObject[] objetos;
    public Transform[] puntosSpawn;
    // Start is called before the first frame update
    void Start()
    {
        SpawnObjetosAleatorios();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnObjetosAleatorios()
    {
        List<int> indicesDisponibles = new List<int>();
        for (int i = 0; i < puntosSpawn.Length; i++)
        {
            indicesDisponibles.Add(i);
        }
        foreach (GameObject objeto in objetos)
        {
            if (indicesDisponibles.Count == 0)
            {
                Debug.LogWarning("No hay suficientes puntos de spawn para todos los objetos.");
                return;
            }
            int indexRandom = Random.Range(0, indicesDisponibles.Count);
            int indexSpawn = indicesDisponibles[indexRandom];
            indicesDisponibles.RemoveAt(indexRandom);
            Instantiate(objeto, puntosSpawn[indexSpawn].position, Quaternion.identity);
        }
    }
}
