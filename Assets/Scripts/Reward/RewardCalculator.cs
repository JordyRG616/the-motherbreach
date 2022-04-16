using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCalculator : MonoBehaviour, ISavable
{

    #region Singleton
    private static RewardCalculator _instance;
    public static RewardCalculator Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<RewardCalculator>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<RewardCalculator>();
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private List<GameObject> initialWeapons;
    public List<GameObject> weapons {get; private set;}
    [SerializeField] private List<GameObject> initialBases;
    public List<GameObject> bases {get; private set;}
    public int ShopLevel 
    {
        get
        {
            return _shopLevel;
        } 
        private set
        {
            if(value < maxShopLevel) _shopLevel = value;
            else _shopLevel = maxShopLevel;
        }
    }
    
    private int _shopLevel = 1;
    public int maxShopLevel;
    private int expAmount;
    private Dictionary<int, int> expRequeriment = new Dictionary<int, int>();
    private RewardManager rewardManager;
    private bool choosing;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string expGained;
    [SerializeField] [FMODUnity.EventRef] private string levelGained;

    void Start()
    {
        rewardManager = RewardManager.Main;

        weapons = new List<GameObject>(initialWeapons);
        bases = new List<GameObject>(initialBases);

        InitiateExpMatrix();
    }

    private void InitiateExpMatrix()
    {
        expRequeriment.Add(1, 2);
        expRequeriment.Add(2, 2);
        expRequeriment.Add(3, 2);
        expRequeriment.Add(4, 3);
        expRequeriment.Add(5, 3);
        expRequeriment.Add(6, 3);
        expRequeriment.Add(7, 4);
        expRequeriment.Add(8, 4);
        expRequeriment.Add(9, 4);
        expRequeriment.Add(10, int.MaxValue);
    }

    public void PurchaseLevelUp()
    { 
        var cost = ShopLevel + 1;

        if(rewardManager.TotalCash >= cost && ShopLevel < maxShopLevel)
        {
            rewardManager.SpendCash(cost);
            GainExp();
            return;
        }
        AudioManager.Main.PlayInvalidSelection();
    }

    public void GainExp()
    {
        expAmount++;
        AudioManager.Main.RequestGUIFX(expGained);
    }

    public void InitiateChoice()
    {
        if(choosing == true) return;
        if(expAmount == expRequeriment[ShopLevel])
        {
            PackOfferManager.Main.IniatiatePackChoice();
            FindObjectOfType<OfferTweaker>().removalPoints ++;
            choosing = true;
        }
    }

    public void LevelUp()
    {
        expAmount = 0;
        ShopLevel++;
        AudioManager.Main.RequestGUIFX(levelGained);
        FindObjectOfType<RerrollButton>().Reroll();
        choosing = false;
    }

    public void ReceiveRewards(List<GameObject> rewards)
    {
        foreach(GameObject reward in rewards)
        {
            if(reward.TryGetComponent<ActionController>(out var controller))
                weapons.Add(reward);
            if(reward.TryGetComponent<BaseEffectTemplate>(out var effect))
                bases.Add(reward);
            if(reward.TryGetComponent<Artifact>(out var artifact))
                ShipManager.Main.ReceiveArtifact(artifact);
        }

        LevelUp();
    }

    public (int amount, int required) ExpInfo()
    {
        return (expAmount, expRequeriment[ShopLevel]);
    }

    public RewardLevel CalculateRewardLevel()
    {
        RewardLevel level = RewardLevel.Common;
        level += ShopLevel - 1;
        return level;
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();

        container.Add("ShopLevel", BitConverter.GetBytes(ShopLevel));
        container.Add("ShopExp", BitConverter.GetBytes(expAmount));
        container.Add("WeaponCount", BitConverter.GetBytes(weapons.Count));
        container.Add("BaseCount", BitConverter.GetBytes(bases.Count));
        container.Add("RemovalPoints", BitConverter.GetBytes(FindObjectOfType<OfferTweaker>().removalPoints));
        
        int i = 0;
        foreach(GameObject weapon in weapons)
        {
            container.Add("ShopWeapon" + i, BitConverter.GetBytes(weapon.GetComponent<ActionController>().weaponID));
            i++;
        }

        i = 0;
        foreach(GameObject _base in bases)
        {
            container.Add("ShopBase" + i, BitConverter.GetBytes(_base.GetComponent<BaseEffectTemplate>().baseID));
            i++;
        }

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        ShopLevel = BitConverter.ToInt32(saveFile.GetValue("ShopLevel"));
        expAmount = BitConverter.ToInt32(saveFile.GetValue("ShopExp"));

        var w_count = BitConverter.ToInt32(saveFile.GetValue("WeaponCount"));
        var b_count = BitConverter.ToInt32(saveFile.GetValue("BaseCount"));

        weapons.Clear();
        bases.Clear();

        for (int i = 0; i < w_count; i++)
        {
            var id = BitConverter.ToInt32(saveFile.GetValue("ShopWeapon" + i));
            weapons.Add(TurretConstructor.Main.GetWeaponPrefabById(id));
        }

        for (int i = 0; i < b_count; i++)
        {
            var id = BitConverter.ToInt32(saveFile.GetValue("ShopBase" + i));
            bases.Add(TurretConstructor.Main.GetBasePrefabById(id));
        }

        var removalCount = BitConverter.ToInt32(saveFile.GetValue("RemovalPoints"));

        var offerTweaker = FindObjectOfType<OfferTweaker>();

        if(offerTweaker == null) return;

        for (int i = 0; i < removalCount; i++)
        {
            offerTweaker.removalPoints++;
        }

    }
}
