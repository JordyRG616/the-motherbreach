using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionData
{
    public LayerMask targetLayer;
    public float speed;
    public float range;
    public float bulletSize;
    public float cooldown;
    public float damage;
    private Dictionary<ActionStat, float> stats = new Dictionary<ActionStat, float>();

    private void InitiateList()
    {
        stats.Add(ActionStat.Speed, speed);
        stats.Add(ActionStat.Range, range);
        stats.Add(ActionStat.Size, bulletSize);
        stats.Add(ActionStat.Cooldown, cooldown);
        stats.Add(ActionStat.Damage, damage);
    }

    public void SetStat(ActionStat stat, float value)
    {
        if(stats.Count == 0)
        {
            InitiateList();
        }

        stats[stat] = value;
    }
}
