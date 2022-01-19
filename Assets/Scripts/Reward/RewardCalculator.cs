using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCalculator : MonoBehaviour
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
    [SerializeField] private int maxShopLevel;
    private int expAmount;
    private Dictionary<int, int> expRequeriment = new Dictionary<int, int>();
    private RewardManager rewardManager;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string expGained;
    [SerializeField] [FMODUnity.EventRef] private string levelGained;

    void Start()
    {
        rewardManager = RewardManager.Main;
        InitiateExpMatrix();
    }

    private void InitiateExpMatrix()
    {
        expRequeriment.Add(1, 2);
        expRequeriment.Add(2, 2);
        expRequeriment.Add(3, 3);
        expRequeriment.Add(4, 4);
        expRequeriment.Add(5, 0);
    }

    public void PurchaseLevelUp()
    { 
        var cost = ShopLevel + 1;

        if(rewardManager.TotalCash >= cost && ShopLevel < maxShopLevel)
        {
            rewardManager.SpendCash(cost);
            expAmount ++;
            AudioManager.Main.RequestGUIFX(expGained);
            return;
        }
        AudioManager.Main.PlayInvalidSelection();
    }

    public void LevelUp()
    {
        if(expAmount == expRequeriment[ShopLevel])
        {
            expAmount = 0;
            ShopLevel++;
            AudioManager.Main.RequestGUIFX(levelGained);
        }
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
}
