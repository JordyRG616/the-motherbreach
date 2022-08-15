using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Wave/Modifiers/Phased", fileName = "Phased Wave")]
public class PhasedWave : WaveModifier
{
    [SerializeField] private LayerMask layer;

    public override void ApplyFormationEffect(FormationManager manager)
    {
        foreach (var child in manager.Children)
        {
            var rdm = Random.Range(0, 1f);
            if (rdm < chanceToApply)
            {
                child.gameObject.layer = layer;
                child.GetComponent<SpriteRenderer>().material.SetInt("_Phased", 1);
            }
        }
    }

    public override void ApplyWaveModifier(WaveData wave)
    {
    }
}
