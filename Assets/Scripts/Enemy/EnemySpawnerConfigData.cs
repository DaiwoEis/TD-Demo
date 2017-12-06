using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ConfigData/Enemy/EnemySpawnerData")]
public class EnemySpawnerConfigData : ConfigData
{
    public EnemyWave[] Waves;

    public float WaveInterval = 5f;

    public float WaveIntervalChanged = 5f;

    public float EnemySpawnInterval = 2f;

    public float MoveSpeedAdjustMin = 0.8f;

    public float MoveSpeedAdjustMax = 1.2f;

    public int WaveCount { get { return Waves.Length; } }

    public override void Init()
    {
        base.Init();

        foreach (var enemyWave in Waves)
        {
            float rangeMin = 0f;
            foreach (var spawnData in enemyWave.SpawnDatas)
            {
                spawnData.Prefab = ResourceManager.Load<GameObject>(spawnData.PrefabName);
                spawnData.ProbabilityRange = new Range(rangeMin, rangeMin + spawnData.SpawnProbability);

                rangeMin += spawnData.SpawnProbability;
            }
        }
    }

    public EnemyWave GetWave(int index)
    {
        if (index <= 0 || index > WaveCount)
            return null;

        return Waves[index - 1];
    }
}

[Serializable]
public class EnemyWave
{
    public EnemySpawnData[] SpawnDatas;

    public int SpawnCount;
}

[Serializable]
public class EnemySpawnData
{
    public string PrefabName;

    public float SpawnProbability;

    public Range ProbabilityRange;

    [HideInInspector]
    public GameObject Prefab;
}