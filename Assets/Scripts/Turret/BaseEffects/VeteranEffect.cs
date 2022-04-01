using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeteranEffect : BaseEffectTemplate
{
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] [FMODUnity.EventRef] private string sfx;

    public override void ApplyEffect()
    {
        vfx.Play();
        AudioManager.Main.RequestGUIFX(sfx);

        for (int i = 0; i < 3; i++)
        {
            Invoke("LevelUp", i/10f);
        }
    }

    private void LevelUp()
    {
        turretManager.LevelUp();
    }

    public override string DescriptionText()
    {
        string description = "Immediately level up this turret three times";
        return description;
    }
}
