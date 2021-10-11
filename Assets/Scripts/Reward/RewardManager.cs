using System;
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

    private Dictionary<OfferBox, GameObject> turretsInOffer = new Dictionary<OfferBox, GameObject>();
    public GameObject ActiveSelection {get; private set;}
    

    void Start()
    {
        Initiate();
    }

    public void Initiate()
    {
        turretConstructor = TurretConstructor.Main;
        calculator = RewardCalculator.Main;
        //waveManager = WaveManager.Main;
        guiManager = RewardGUIManager.Main;

        guiManager.InitiateGUI();
        GenerateOffer();
    }

    private void GenerateOffer()
    {
        foreach(OfferBox box in guiManager.GetBoxes())
        {
            if(box.Empty == true)
            {
                GenerateReward(box);
            }
        }
    }

    public void BuildSelection()
    {
        Destroy(ActiveSelection);
        guiManager.TerminateGUI();
    }

    private void AddToOffer(GameObject turret, OfferBox box)
    {
        box.ReceiveTurret(turret);
        box.OnOfferSelected += SelectTurret;
        turret.transform.position = box.transform.position - Vector3.left * 100;
        turretsInOffer.Add(box, turret);
    }

    private void SelectTurret(object sender, EventArgs e)
    {
        OfferBox box = (OfferBox)sender;
        ActiveSelection = turretsInOffer[box];
        ActiveSelection.AddComponent<TrackingDevice>().StartTracking();
        ActiveSelection.GetComponentInChildren<TurretVFXManager>().EnableSelected();
    }

    private void GenerateReward(OfferBox box)
    {
        //int waveLevel = waveManager.GetWaveLevel();
        RewardLevel _base = RewardLevel.Common; //calculator.CalculateRewardLevel(waveLevel);
        RewardLevel _top = RewardLevel.Common; //calculator.CalculateRewardLevel(waveLevel);
        AddToOffer(turretConstructor.Construct(_base, _top), box);
    }
}
