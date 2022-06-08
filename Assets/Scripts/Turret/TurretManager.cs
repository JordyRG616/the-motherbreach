using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour, IManager, ISavable
{
    
    public Foundation foundation {get; private set;}
    public Weapon weapon { get; private set; }
    public IntegrityManager integrityManager {get; private set;}


    public int maxLevel = 10;
    public int Level 
    {
        get
        {
            return _level;
        }
        set
        {
            if(value > maxLevel) value = maxLevel;
            _level = value;
        }
    }
    private int _level = 0;
    public int upgradePoints;


    public event EventHandler<LevelUpArgs> OnLevelUp;
    public string slotId;

    public void Initiate()
    {
        weapon = GetComponentInChildren<Weapon>();
        weapon.Initiate();
        foundation = GetComponentInChildren<Foundation>();
        foundation.Initiate();

        integrityManager = GetComponent<IntegrityManager>();
    }

    public void ReceiveInitialRotation(float rotation)
    {
        //actionController.GetShooters().ForEach(x => x.initialRotation = rotation);
    }

    public void DestroyManager()
    {
        GetComponentInParent<ShipManager>().RemoveTurret(this);
    }

    public void LevelUp()
    {
        upgradePoints++;
        Level++;
    }


    public Dictionary<string, byte[]> GetData()
    {
        Dictionary<string, byte[]> container = new Dictionary<string, byte[]>();

        container.Add(slotId + "weaponLevel", BitConverter.GetBytes(Level));
        
        //var weaponData = actionController.GetData();

        //foreach(string key in weaponData.Keys)
        //{
        //    container.Add(slotId + key, weaponData[key]);
        //}

        //container.Add(slotId + "baseCount", BitConverter.GetBytes(baseHistory.Count));

        //int i = 0;

        //foreach(BaseEffectTemplate _b in baseHistory)
        //{
        //    container.Add(slotId + "base" + i, BitConverter.GetBytes(_b.baseID));
        //    i++;
        //}

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        var loadedLevel = BitConverter.ToInt32(saveFile.GetValue(slotId + "weaponLevel"));

        var baseCount = BitConverter.ToInt32(saveFile.GetValue(slotId + "baseCount"));

        for (int i = 1; i < baseCount; i++)
        {
            var baseId = BitConverter.ToInt32(saveFile.GetValue(slotId + "base" + i));
            var _b = TurretConstructor.Main.GetBaseById(baseId);

            //TurretConstructor.Main.ReplaceBase(this.gameObject, _b);
        }

        //for(int i = 1; i <= loadedLevel; i++)
        //{
        //    LevelUp();
        //}
        
        //actionController.LoadData(saveFile, slotId);
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