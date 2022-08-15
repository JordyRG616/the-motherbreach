using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Wave/Modifiers/Shielded", fileName = "Shielded Wave")]
public class ShieldedWave : WaveModifier
{
    [SerializeField] private GameObject shield;

    public override void ApplyFormationEffect(FormationManager manager)
    {
        foreach (var child in manager.Children)
        {
            var rdm = Random.Range(0, 1f);
            if(rdm < chanceToApply)
            {
                Instantiate(shield, child.transform).transform.localPosition = Vector3.zero;
            }
        }
    }

    public override void ApplyWaveModifier(WaveData wave)
    {
    }
}
