using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ConfigData/Defender/DefenderRocketConfigData")]
public class DefenderRocketConfigData : DefenderConfigData
{
    [ShowInInspector]
    public ResourceLoader<GameObject> RocketPrefabLoader;

    [DisableInEditorMode]
    public GameObject RocketPrefab;

    public override void Init()
    {
        base.Init();

        RocketPrefab = RocketPrefabLoader.Load();
    }
}
