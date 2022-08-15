using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Unlockables/Training Algorithym", fileName = "Training Algorithym")]
public class TrainingAlgorithym : Trait
{
    public int gainedLevels;

    public override void ApplyEffect(Weapon weapon)
    {
        var manager = weapon.GetComponentInParent<TurretManager>();

        for(int i = 0; i < gainedLevels; i++)
        {
            manager.LevelUp();
        }
    }

    public override string Description()
    {
        return "This turret receives three levels";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (TrainingAlgorithym)ScriptableObject.CreateInstance(type);

        instance.gainedLevels = gainedLevels;

        return instance;
    }
}
