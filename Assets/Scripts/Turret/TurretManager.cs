using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour, IManager, ISavable
{
    
    public BaseEffectTemplate baseEffect {get; private set;}
    public ActionController actionController {get; private set;}
    public Dictionary<Stat, float> Stats {get; protected set;} = new Dictionary<Stat, float>();
    public IntegrityManager integrityManager {get; private set;}

    private List<BaseEffectTemplate> baseHistory = new List<BaseEffectTemplate>();

    public int maxLevel = 5;
    public int Level 
    {
        get
        {
            return _level;
        }
        private set
        {
            if(value > 5) value = 5;
            _level = value;
        }
    }
    private int _level = 0;


    public event EventHandler<LevelUpArgs> OnLevelUp;
    public string slotId;


    public void Initiate()
    {
        baseEffect = GetComponentInChildren<BaseEffectTemplate>();
        actionController = GetComponentInChildren<ActionController>();

        baseHistory.Add(baseEffect);

        OnLevelUp += actionController.HandleLevelUp;

        GetStats();

        integrityManager = GetComponent<IntegrityManager>();
        integrityManager.Initiate(Stats[Stat.Health]);
    }

    public void ReceiveInitialRotation(float rotation)
    {
        actionController.GetShooters().ForEach(x => x.initialRotation = rotation);
    }

    private void GetStats()
    {
        Stats.Add(Stat.Cost, baseEffect.GetCost() + actionController.GetCost());
        Stats.Add(Stat.Health, actionController.GetHealth());

        var stats = actionController.GetShooters()[0].StatSet;

        foreach(Stat stat in stats.Keys)
        {
            Stats.Add(stat, stats[stat]);
        }

    }

    public void LevelUp()
    {
        Level ++;
        actionController.RaiseHealthByPercentage(.1f);
        OnLevelUp?.Invoke(this, new LevelUpArgs(Level));
        actionController.SaveStats();
    }

    public void DestroyManager()
    {
        GetComponentInParent<ShipManager>().RemoveTurret(this);
    }

    public void ReplaceBase(BaseEffectTemplate newBase)
    {
        Destroy(baseEffect.gameObject);
        baseEffect = newBase;
        baseHistory.Add(baseEffect);
    }

    public Dictionary<string, byte[]> GetData()
    {
        Dictionary<string, byte[]> container = new Dictionary<string, byte[]>();

        container.Add(slotId + "weaponLevel", BitConverter.GetBytes(Level));
        
        var weaponData = actionController.GetData();

        foreach(string key in weaponData.Keys)
        {
            container.Add(slotId + key, weaponData[key]);
        }

        container.Add(slotId + "baseCount", BitConverter.GetBytes(baseHistory.Count));

        int i = 0;

        foreach(BaseEffectTemplate _b in baseHistory)
        {
            container.Add(slotId + "base" + i, BitConverter.GetBytes(_b.baseID));
            i++;
        }

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        var loadedLevel = BitConverter.ToInt32(saveFile.GetValue(slotId + "weaponLevel"));
        Level = loadedLevel;

        actionController.LoadData(saveFile, slotId);

        var baseCount = BitConverter.ToInt32(saveFile.GetValue(slotId + "baseCount"));

        for (int i = 1; i < baseCount; i++)
        {
            var baseId = BitConverter.ToInt32(saveFile.GetValue(slotId + "base" + i));
            var _b = TurretConstructor.Main.GetBaseById(baseId);

            TurretConstructor.Main.ReplaceBase(this.gameObject, _b);
        }
    }
}

public class LevelUpArgs : EventArgs
{
    public int toLevel;

    public LevelUpArgs(int level)
    {
        toLevel = level;
    }
}