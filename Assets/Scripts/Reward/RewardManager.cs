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

    [SerializeField] private List<RewardBox> rewardBoxes;
    private TurretConstructor turretConstructor;
    private RewardCalculator calculator;
    private WaveManager waveManager;
    private UIAnimationManager animationManager;

    private Dictionary<RewardBox, GameObject> turretsInOffer = new Dictionary<RewardBox, GameObject>();
    public GameObject ActiveSelection {get; private set;}

    public event EventHandler OnRewardSelection;

    public void Initialize()
    {
        turretConstructor = TurretConstructor.Main;
        turretConstructor.Initialize();
        calculator = RewardCalculator.Main;
        animationManager = UIAnimationManager.Main;
    }

    public void InitiateReward()
    {
        animationManager.InitiateUI();
        GenerateOffer();
    }

    private void GenerateOffer()
    {
        foreach(RewardBox box in rewardBoxes)
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
        EliminateOffer();
        animationManager.TerminateUI();
        OnRewardSelection?.Invoke(this, EventArgs.Empty);
    }

    private void EliminateOffer()
    {
        foreach(GameObject turret in turretsInOffer.Values)
        {
            Destroy(turret);
        }

        foreach(RewardBox box in turretsInOffer.Keys)
        {
            box.Clear();
        }

        turretsInOffer.Clear();
    }

    private void AddToOffer(GameObject turret, RewardBox box)
    {
        box.ReceiveTurret(turret);
        box.OnOfferSelected += SelectTurret;
        turret.transform.position = box.transform.position - Vector3.left * 100;
        turretsInOffer.Add(box, turret);
    }

    private void SelectTurret(object sender, EventArgs e)
    {
        RewardBox box = (RewardBox)sender;
        ActiveSelection = turretsInOffer[box];
        ActiveSelection.AddComponent<TrackingDevice>().StartTracking();
        foreach (SpriteRenderer renderer in ActiveSelection.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = Color.white;
        }
        ActiveSelection.GetComponentInChildren<TurretVFXManager>().EnableSelected();
    }

    private void GenerateReward(RewardBox box)
    {
        //int waveLevel = waveManager.GetWaveLevel();
        RewardLevel _base = RewardLevel.Common; //calculator.CalculateRewardLevel(waveLevel);
        RewardLevel _top = RewardLevel.Common; //calculator.CalculateRewardLevel(waveLevel);
        AddToOffer(turretConstructor.Construct(_base, _top), box);
    }
}
