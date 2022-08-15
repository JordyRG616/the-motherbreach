using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaveModifier : ScriptableObject
{
    public float chanceToApply;

    public abstract void ApplyFormationEffect(FormationManager manager);
    public abstract void ApplyWaveModifier(WaveData wave);
}
