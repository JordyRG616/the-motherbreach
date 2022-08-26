using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableShieldSpeed : BulletSpeed
{
    [SerializeField] private List<PortableShield> portableShields;

    protected override void SetValue(float value)
    {
        portableShields.ForEach(x => x.SetOrbitSpeed(value));
    }

    public override string GetLiteralValue()
    {
        return (Value * 100).ToString();
    }

    public override string GetLiteralStartingValue()
    {
        return (startingValue * 100).ToString();
    }
}
