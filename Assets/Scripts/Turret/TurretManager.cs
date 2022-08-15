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

    public delegate void TurretDestroyedEvent();
    public TurretDestroyedEvent OnTurretDestruction;

    public void Initiate()
    {
        weapon = GetComponentInChildren<Weapon>();
        foundation = GetComponentInChildren<Foundation>();
        weapon.Initiate();
        foundation.Initiate(weapon);

        integrityManager = GetComponent<IntegrityManager>();
    }

    public void ReceiveInitialRotation(float rotation)
    {
        //actionController.GetShooters().ForEach(x => x.initialRotation = rotation);
    }

    public void DestroyManager()
    {
        GetComponentInParent<ShipManager>().RemoveTurret(this);

        OnTurretDestruction?.Invoke();
    }

    public void LevelUp()
    {
        upgradePoints++;
        Level++;

        OnLevelUp?.Invoke(this, new LevelUpArgs(Level));
    }

    public Dictionary<string, byte[]> GetData()
    {
        Dictionary<string, byte[]> container = new Dictionary<string, byte[]>();

        container.Add(slotId + "weaponLevel", BitConverter.GetBytes(Level));
        container.Add(slotId + "upgradePoints", BitConverter.GetBytes(upgradePoints));
        container.Add(slotId + "Foundation", BitConverter.GetBytes(foundation.Id));

        var weaponData = weapon.GetData();

        foreach (string key in weaponData.Keys)
        {
            container.Add(key, weaponData[key]);
        }

        var foundationData = foundation.GetData();

        foreach (string key in foundationData.Keys)
        {
            container.Add(key, foundationData[key]);
        }


        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        Level = BitConverter.ToInt32(saveFile.GetValue(slotId + "weaponLevel"));

        upgradePoints = BitConverter.ToInt32(saveFile.GetValue(slotId + "upgradePoints"));

        weapon.LoadData(saveFile);
        foundation.LoadData(saveFile);
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