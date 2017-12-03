using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public int enemyTotal = 50;

    public EnemySpawnerData Data;

    private Coroutine _spawnCoroutine;

    private int _currWave;

    public void Start()
    {
        Data.Init();

        StartSpawn();
    }

    public void StartSpawn()
    {
        _spawnCoroutine = StartCoroutine(_StartSpawn());
    }

    public void StopSpawn()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private IEnumerator _StartSpawn()
    {
        _currWave = 1;
        float waveInterval = Data.WaveInterval;
        while (_currWave <= Data.WaveCount)
        {
            var currWaveData = Data.GetWave(_currWave);
            for (int currWaveSpawnedEnemy = 0; currWaveSpawnedEnemy < currWaveData.SpawnCount; ++currWaveSpawnedEnemy)
            {
                SpawnEnemy(GetRandomPrefab(currWaveData));
                if (currWaveSpawnedEnemy != currWaveData.SpawnCount - 1)
                    yield return new WaitForSeconds(Data.EnemySpawnInterval);
            }

            yield return new WaitForSeconds(waveInterval);
            waveInterval += Data.WaveIntervalChanged;
            _currWave++;
        }
        _spawnCoroutine = null;
    }

    private GameObject GetRandomPrefab(EnemyWave enemyWave)
    {
        float rand = Random.Range(0f, 1f);
        foreach (var spawnData in enemyWave.SpawnDatas)
        {
            if (spawnData.ProbabilityRange.WithInRange(rand))
                return spawnData.Prefab;
        }

        return null;
    }

    private void SpawnEnemy(GameObject enemyGO)
    {
        GameObject enemyGo = EntityManager.Instance.CreateGO(enemyGO, transform.position, Quaternion.identity, true);
        Enemy enemyStatus = enemyGo.GetComponent<Enemy>();
        enemyStatus.moveSpeed *= Random.Range(Data.MoveSpeedAdjustMin, Data.MoveSpeedAdjustMax);
        enemyStatus.HP = enemyStatus.HP * (1 + _currWave / Data.WaveCount);
    }

}
