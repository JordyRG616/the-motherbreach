using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Unlockables/Scrap Collector", fileName = "Scrap Collector")]
public class ScrapCollector : Trait
{
    private WaveManager waveManager;

    public override void ApplyEffect(Weapon weapon)
    {
        waveManager = WaveManager.Main;
        waveManager.OnWaveEnd += GainReward;
    }

    private void GainReward(object sender, EndWaveEventArgs e)
    {
        RewardManager.Main.TotalCash += 1;
    }

    public override string Description()
    {
        return "Receive +1$ at the end of the wave";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (ScrapCollector)ScriptableObject.CreateInstance(type);

        return instance;
    }
}
