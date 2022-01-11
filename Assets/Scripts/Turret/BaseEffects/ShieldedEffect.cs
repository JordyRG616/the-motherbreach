using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedEffect : BaseEffectTemplate
{
    [SerializeField] private GameObject shield;

    public override void ApplyEffect()
    {
        Instantiate(shield, Vector3.zero, Quaternion.identity, associatedController.transform).transform.localPosition = Vector3.zero;
    }

    public override string DescriptionText()
    {
        string description = "";
        return description;
    }
}
