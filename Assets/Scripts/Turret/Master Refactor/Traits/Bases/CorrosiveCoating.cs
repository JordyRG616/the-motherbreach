using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Bases/Corrosive Coating", fileName = "Corrosive Coating")]
public class CorrosiveCoating : Trait
{
    public float duration;
    public float acidDamage;
    public float frequency;

    public override void ApplyEffect(Weapon weapon)
    {
        weapon.totalEffect += ApplyAcid;
    }

    private void ApplyAcid(HitManager manager)
    {
        if (manager.IsUnderEffect<Acid>(out var status)) status.DestroyEffect();
        var effect = manager.gameObject.AddComponent<Acid>();
        effect.Initialize(manager, duration, new float[] { acidDamage, frequency });
    }

    public override string Description()
    {
        return "This turret's bullets will applye ACID on hit";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (CorrosiveCoating)ScriptableObject.CreateInstance(type);

        instance.duration = duration;
        instance.acidDamage = acidDamage;
        instance.frequency = frequency;

        return instance;
    }
}
