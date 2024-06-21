using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo para spawnear
    public List<Transform> spawnPoints; // Lista de puntos de spawn
    public float spawnInterval = 3f; // Intervalo de tiempo entre spawns en segundos
    private float timer; // Temporizador para controlar el spawn

    void Start()
    {
        timer = spawnInterval; // Inicializa el temporizador

        // Spawn inicial de enemigos al comenzar el juego
        SpawnInitialEnemies();
    }

    void Update()
    {
        timer -= Time.deltaTime; // Actualiza el temporizador cada frame
        if (timer <= 0)
        {
            SpawnEnemy(); // Función para spawnear un enemigo
            timer = spawnInterval; // Restablece el temporizador
        }
    }

    void SpawnInitialEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Instancia un enemigo en cada punto de spawn al iniciar el juego
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Count > 0)
        {
            // Selecciona un punto de spawn aleatorio de la lista
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation); // Instancia el enemigo en el punto seleccionado
        }
        else
        {
            Debug.LogError("No spawn points assigned!");
        }
    }
}
