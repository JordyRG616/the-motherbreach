using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Wave/Modifiers/Assault", fileName = "Assault Wave")]
public class AssaultWave : WaveModifier
{
    public override void ApplyFormationEffect(FormationManager manager)
    {
    }

    public override void ApplyWaveModifier(WaveData wave)
    {
        wave.EnqueueExtraWaves(2);
    }
}
