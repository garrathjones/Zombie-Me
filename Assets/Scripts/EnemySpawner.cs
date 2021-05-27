using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;
    [SerializeField] float spawnPositionOffsetRandomness = 3f;

    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }

    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            GameObject zombie = Instantiate(waveConfig.GetEnemyPrefab(), transform.position, transform.rotation);
            float randPosOffset = Random.Range(-spawnPositionOffsetRandomness, spawnPositionOffsetRandomness);
            var randomPosition = new Vector2(zombie.transform.position.x + randPosOffset, zombie.transform.position.y + randPosOffset);
            zombie.transform.position = randomPosition;      
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }

    }

}
