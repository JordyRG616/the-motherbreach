using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableShieldSize : Size
{
    [SerializeField] private List<PortableShield> portableShields;

    protected override void SetValue(float value)
    {
        portableShields.ForEach(x => x.SetSize(value));
    }
}
