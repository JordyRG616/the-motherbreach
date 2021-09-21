using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    #region Singleton
    private static RewardManager _instance;
    public static RewardManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<RewardManager>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<RewardManager>();
                }
            }
            return _instance;
        }
    }
    #endregion



    private TurretConstructor turretConstructor;
    private RewardCalculator calculator;
    private WaveManager waveManager;
    private RewardGUIManager guiManager;

    private List<GameObject> turretsInOffer = new List<GameObject>();
    

    void Start()
    {
        Initiate();
    }

    public void Initiate()
    {
        turretConstructor = TurretConstructor.Main;
        calculator = RewardCalculator.Main;
        waveManager = WaveManager.Main;
        guiManager = RewardGUIManager.Main;

        
    }

    private void AddToOffer(GameObject turret)
    {
        turretsInOffer.Add(turret);
    }
    
    

    private void GenerateReward()
    {
        int waveLevel = waveManager.GetWaveLevel();
        RewardLevel _base = calculator.CalculateRewardLevel(waveLevel);
        RewardLevel _top = calculator.CalculateRewardLevel(waveLevel);
        AddToOffer(turretConstructor.Construct(_base, _top));
    }
}
