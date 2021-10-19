using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class ActionData
{
    public LayerMask targetLayer;
    public float Speed;
    public float Range;
    public float Size;
    public float Cooldown;
    public float Damage;
    private Dictionary<ActionStat, float> stats = new Dictionary<ActionStat, float>();

    public void SetStat(ActionStat stat, float value)
    {
        FieldInfo _stat = typeof(ActionData).GetField(stat.ToString());
        _stat.SetValue(this, value);
    }
}
