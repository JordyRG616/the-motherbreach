using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class SpawnerEffect : DeployerActionEffect
{
    [SerializeField] private float droneLevel;

    public override Stat specializedStat => Stat.DroneLevel;

    public override Stat secondaryStat => Stat.Capacity;

    public override void SetData()
    {
        StatSet.Add(Stat.Capacity, capacity);
        StatSet.Add(Stat.DroneLevel, droneLevel);

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
    }
    public override void ApplyEffect(HitManager hitManager)
    {
        
    }

    public override string DescriptionText()
    {
        string description = "spawn up to " + StatColorHandler.StatPaint(StatSet[Stat.Capacity].ToString()) + " drones of level " + StatColorHandler.StatPaint(StatSet[Stat.DroneLevel].ToString());
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " capacity + 1";
        if(nextLevel == 2 || nextLevel == 4) return StatColorHandler.StatPaint("next level:") + " drone level + 1";
        return "no bonus next level";
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5) GainCapacity();
        if(toLevel == 2 || toLevel == 4) GainLevel();
    }

    private void GainLevel()
    {
        var droneLevel = StatSet[Stat.DroneLevel];
        droneLevel += 1;
        SetStat(Stat.DroneLevel, droneLevel);
    }

    private void GainCapacity()
    {
        var capacity = StatSet[Stat.Capacity];
        capacity += 1;
        SetStat(Stat.Capacity, capacity);
    }
}
