using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Unlockables/Machine Learning", fileName = "Machine Learning")]
public class MachineLearning : Trait
{
    private int counter;
    private TurretManager turretManager;
    private WaveManager waveManager;

    public override void ApplyEffect(Weapon weapon)
    {
        turretManager = weapon.GetComponentInParent<TurretManager>();
        waveManager = WaveManager.Main;

        waveManager.OnWaveEnd += UpdateCounter;
        turretManager.OnTurretDestruction += DestroyTrait;
    }

    private void DestroyTrait()
    {
        waveManager.OnWaveEnd -= UpdateCounter;
    }

    private void UpdateCounter(object sender, EndWaveEventArgs e)
    {
        counter++;
        if(counter == 2)
        {
            turretManager.upgradePoints++;
            counter = 0;
        }
    }

    public override string Description()
    {
        return "This turret receives an upgrade point every two waves";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (MachineLearning)ScriptableObject.CreateInstance(type);

        //instance.percentage = percentage;

        return instance;
    }
}
