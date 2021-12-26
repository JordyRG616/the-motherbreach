using System.Linq;
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

    private List<RewardBox> rewardBoxes;
    private UIAnimations cashTextAnimation;

    private InputManager inputManager;
    private TurretConstructor turretConstructor;
    private RewardCalculator calculator;
    private WaveManager waveManager;
    private UIAnimationManager animationManager;
     public float TotalCash, EarnedCash, SpendedCash;

    private Dictionary<RewardBox, GameObject> turretsInOffer = new Dictionary<RewardBox, GameObject>();
    public GameObject ActiveSelection {get; private set;}
    private RewardBox activeBox;

    public event EventHandler OnRewardSelection;

    public void Initialize()
    {
        turretConstructor = TurretConstructor.Main;
        turretConstructor.Initialize();
        calculator = RewardCalculator.Main;
        animationManager = UIAnimationManager.Main;

        inputManager = InputManager.Main;
        inputManager.OnSelectionClear += ClearSelection;

        rewardBoxes = FindObjectsOfType<RewardBox>(true).ToList();
        cashTextAnimation = FindObjectOfType<CashTextAnimation>();
    }

    public void InitiateReward(float rewardValue)
    {
        SpendedCash = 0;
        EarnedCash = rewardValue;
        animationManager.InitiateUI();

        var locked = FindObjectOfType<LockButton>().locked;

        if(!locked) GenerateOffer();
    }

    public void GenerateOffer()
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
        SpendedCash = ActiveSelection.GetComponent<TurretManager>().Stats[Stat.Cost];
        cashTextAnimation.PlayReverse();

        // foreach(TrackingDevice device in ActiveSelection.GetComponents<TrackingDevice>())
        // {
        //     Destroy(device);
        // }

        ActiveSelection = null;        

        turretsInOffer.Remove(activeBox);
        activeBox.Clear();
        activeBox = null;
    }

    public void Exit()
    {
        var locked = FindObjectOfType<LockButton>().locked;

        if(!locked) EliminateOffer();
        // animationManager.TerminateUI();
        OnRewardSelection?.Invoke(this, EventArgs.Empty);
    }

    public void EliminateOffer()
    {
        foreach(GameObject turret in turretsInOffer.Values)
        {
            Destroy(turret);
        }

        foreach(RewardBox box in turretsInOffer.Keys)
        {
            box.OnOfferSelected -= SelectTurret;
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
        turret.SetActive(false);
    }

    private void SelectTurret(object sender, EventArgs e)
    {
        if(ActiveSelection != null)
        {
            ClearSelection();
        }
        activeBox = (RewardBox)sender;
        if(turretsInOffer[activeBox].GetComponent<TurretManager>().Stats[Stat.Cost] <= TotalCash)
        {
            ActiveSelection = turretsInOffer[activeBox];
            ActiveSelection.SetActive(true);
            ActiveSelection.AddComponent<TrackingDevice>().StartTracking();
            foreach (SpriteRenderer renderer in ActiveSelection.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.color = Color.white;
            }
            ActiveSelection.GetComponentInChildren<TurretVFXManager>().EnableSelected();
        }

        activeBox.OnOfferSelected -= SelectTurret;
    }

    private void ClearSelection()
    {
        if(ActiveSelection != null) ActiveSelection.SetActive(false);
        ActiveSelection = null;
    }

    private void ClearSelection(object sender, EventArgs e)
    {
        if(ActiveSelection != null) ActiveSelection.SetActive(false);
        Destroy(ActiveSelection.GetComponent<TrackingDevice>());
        activeBox.OnOfferSelected += SelectTurret;
        ActiveSelection = null;
    }

    private void GenerateReward(RewardBox box)
    {
        //int waveLevel = waveManager.GetWaveLevel();
        RewardLevel _base = RewardLevel.Common; //calculator.CalculateRewardLevel(waveLevel);
        RewardLevel _top = RewardLevel.Common; //calculator.CalculateRewardLevel(waveLevel);
        AddToOffer(turretConstructor.Construct(_base, _top), box);
    }
}
