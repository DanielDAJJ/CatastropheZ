using UnityEngine;

public class SpawnRandomZombies : MonoBehaviour
{
  [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;  // Array de prefabs de enemigos

    [Header("Spawn Area Parameters")]
    public Vector3 spawnAreaMin = new Vector3(-10f, 0f, -10f);  // Esquina inferior izquierda del área
    public Vector3 spawnAreaMax = new Vector3(10f, 0f, 10f);    // Esquina superior derecha del área

    [Header("Spawn Parameters")]
    public int numberOfEnemies = 25;  // Número de enemigos a instanciar

    // Start se llama antes de la primera actualización
    void Start()
    {
        SpawnEnemies();
    }

    // Método para instanciar los enemigos
    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Elegir un prefab aleatorio de los disponibles
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Generar una posición aleatoria dentro del área definida
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y), // Si también quieres usar altura aleatoria (Y)
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            // Instanciar el enemigo en la posición aleatoria
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }
}
