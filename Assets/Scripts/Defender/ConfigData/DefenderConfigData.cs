using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ConfigData/Defender/DefenderData")]
public class DefenderConfigData : ConfigData 
{
    public int Atk = 50;

    public int UpgradeAtkChanged = 10;

    public int AtkDistance = 20;

    public int UpgradeAtkDistanceChanged = 10;

    public float AtkIntervial = 0.5f;

    public float UpgradeAtkIntervialChanged = 1.2f;

    public float ScanIntervial = 0.1f;

    [ShowInInspector] public LevelArray<int> BuyPrices = new LevelArray<int>(new[] {250, 350, 450});

    public int MaxLevel { get { return BuyPrices.Length; } }

    public DefenderAnimationName AnimName;

    public float FireTime = 0f;
}
