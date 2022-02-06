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

    
    private List<WeaponBox> weaponBoxes;
    private List<BaseBox> baseBoxes;
    private UIAnimations cashTextAnimation;

    private InputManager inputManager;
    private TurretConstructor turretConstructor;
    private RewardCalculator calculator;
    private UIAnimationManager animationManager;
    public float TotalCash, EarnedCash, SpendedCash;
    private BuildBox buildBox;
    private TutorialManager tutorialManager;

    public GameObject ActiveSelection {get; private set;}

    public event EventHandler OnRewardSelection;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string buildSFX;


    public void Initialize()
    {
        turretConstructor = TurretConstructor.Main;
        turretConstructor.Initialize();
        calculator = RewardCalculator.Main;
        animationManager = GameObject.FindGameObjectWithTag("RewardAnimation").GetComponent<UIAnimationManager>();

        buildBox = FindObjectOfType<BuildBox>();
        tutorialManager = FindObjectOfType<TutorialManager>();

        inputManager = InputManager.Main;
        inputManager.OnSelectionClear += ClearSelection;

        weaponBoxes = FindObjectsOfType<WeaponBox>(true).ToList();
        baseBoxes = FindObjectsOfType<BaseBox>(true).ToList();
        cashTextAnimation = FindObjectOfType<CashTextAnimation>();

        AudioManager.Main.RequestMusic("Reward Song 2");
    }

    public void InitiateReward(float rewardValue)
    {
        SpendedCash = 0;
        EarnCash(rewardValue);
        ShipManager.Main.transform.rotation = Quaternion.identity;
        var ship = FindObjectOfType<ShipController>();
        ship.StopFX();
        ship.transform.rotation = Quaternion.identity;
        ship.GetComponent<Rigidbody2D>().Sleep();
        animationManager.Play();
        
        AudioManager.Main.GetAudioTrack("SFX").PauseAudio();
        AudioManager.Main.GetAudioTrack("Music").PauseAudio();
        AudioManager.Main.GetAudioTrack("Special").UnpauseAudio();


        var locked = FindObjectOfType<LockButton>().locked;

        if(!locked) GenerateOffer();
    }

    public void GenerateOffer()
    {
        RewardLevel _base = calculator.CalculateRewardLevel();
        RewardLevel _top = calculator.CalculateRewardLevel();
        
        foreach(WeaponBox weaponBox in weaponBoxes)
        {
            weaponBox.GenerateNewWeapon(_top);
        }
        foreach(BaseBox baseBox in baseBoxes)
        {
            baseBox.GenerateNewBase(_base);
        }
    }

    public void EarnCash(float amount)
    {
        EarnedCash = amount;
        cashTextAnimation.Play();
    }

    public void SpendCash(float amount)
    {
        SpendedCash = amount;
        cashTextAnimation.PlayReverse();
    }

    public void BuildSelection()
    {
        tutorialManager.TriggerPosBuildTutorial();
        buildBox.Clear();
        turretConstructor.TriggerImeddiateEffect(ActiveSelection);
        var manager = ActiveSelection.GetComponent<TurretManager>();
        SpendCash(manager.Stats[Stat.Cost]);
        ShipManager.Main.RegisterTurret(manager);


        AudioManager.Main.RequestGUIFX(buildSFX);

        ActiveSelection = null;
    }

    public void Exit()
    {
        AudioManager.Main.GetAudioTrack("SFX").UnpauseAudio();
        AudioManager.Main.GetAudioTrack("Special").PauseAudio();
        AudioManager.Main.GetAudioTrack("Music").UnpauseAudio();

        var locked = FindObjectOfType<LockButton>().locked;

        if(!locked) EliminateOffer();
        OnRewardSelection?.Invoke(this, EventArgs.Empty);
    }

    public void EliminateOffer()
    {
        buildBox.Clear();
        foreach(WeaponBox weaponBox in weaponBoxes)
        {
            weaponBox.Clear();
        }
        foreach(BaseBox baseBox in baseBoxes)
        {
            baseBox.Clear();
        }
    }

    public void SetSelection(GameObject _weapon, GameObject _base)
    {
        ActiveSelection = turretConstructor.Construct(_weapon, _base);
        // ActiveSelection.transform.position = Vector3.zero;
        ActiveSelection.SetActive(true);
        ActiveSelection.AddComponent<TrackingDevice>().StartTracking();
        foreach (SpriteRenderer renderer in ActiveSelection.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = Color.white;
        }
        foreach(TurretVFXManager vfx in ActiveSelection.GetComponentsInChildren<TurretVFXManager>())
        {
            vfx.EnableSelected();
        }

    }

    private void ClearSelection()
    {
        if(ActiveSelection != null) ActiveSelection.SetActive(false);
        ActiveSelection = null;
    }

    private void ClearSelection(object sender, EventArgs e)
    {
        if(ActiveSelection == null) return;
        ActiveSelection.GetComponentInChildren<ActionController>().gameObject.SetActive(false);
        ActiveSelection.GetComponentInChildren<ActionController>(true).transform.parent = null;
        ActiveSelection.GetComponentInChildren<BaseEffectTemplate>().gameObject.SetActive(false);
        ActiveSelection.GetComponentInChildren<BaseEffectTemplate>(true).transform.parent = null;
        Destroy(ActiveSelection);
    }

}
