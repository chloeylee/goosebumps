using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnInterval;
}

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;

    private int currentWaveIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnWaveCoroutine()
    {
        while (currentWaveIndex < waves.Length)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            Wave currentWave = waves[currentWaveIndex];
            Debug.Log("Spawning Wave: " + currentWave.waveName);

            for (int i = 0; i < currentWave.enemyCount; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(currentWave.enemyPrefab, spawnPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(currentWave.spawnInterval);
            }

            currentWaveIndex++;
        }

        Debug.Log("All waves complete!");
    }
}
