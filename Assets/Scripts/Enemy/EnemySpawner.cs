using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public string ConfigDataName;

    private EnemySpawnerConfigData _configData;

    private Coroutine _spawnCoroutine;

    private int _currWave;

    public int EnemyTotal
    {
        get
        {
            int count = 0;
            foreach (var wave in _configData.Waves)
            {
                count += wave.SpawnCount;
            }
            return count;
        }
    }

    public void Awake()
    {
        _configData = ResourceManager.Load<EnemySpawnerConfigData>(ConfigDataName);
        _configData.Init();

        GameController.Instance.OnGameStart += StartSpawn;
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
        float waveInterval = _configData.WaveInterval;
        while (_currWave <= _configData.WaveCount)
        {
            var currWaveData = _configData.GetWave(_currWave);
            for (int currWaveSpawnedEnemy = 0; currWaveSpawnedEnemy < currWaveData.SpawnCount; ++currWaveSpawnedEnemy)
            {
                SpawnEnemy(GetRandomPrefab(currWaveData));
                if (currWaveSpawnedEnemy != currWaveData.SpawnCount - 1)
                    yield return new WaitForSeconds(_configData.EnemySpawnInterval);
            }

            yield return new WaitForSeconds(waveInterval);
            waveInterval += _configData.WaveIntervalChanged;
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
        enemyStatus.MoveSpeed *= Random.Range(_configData.MoveSpeedAdjustMin, _configData.MoveSpeedAdjustMax);
        enemyStatus.HP = enemyStatus.HP * (1 + _currWave / _configData.WaveCount);
    }

}
