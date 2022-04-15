using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour, ISavable
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
    public int TotalCash, EarnedCash, SpendedCash;
    private BuildBox buildBox;
    private TutorialManager tutorialManager;

    public GameObject ActiveSelection {get; private set;}

    public event EventHandler OnRewardSelection;
    public event EventHandler<BuildEventArgs> OnTurretBuild;

    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string buildSFX;


    public void ClearEvents()
    {
        if(OnRewardSelection != null)
        {
            foreach(Delegate d in OnRewardSelection.GetInvocationList())
            {
                OnRewardSelection -= (EventHandler)d;
            }
        }

        if(OnTurretBuild != null)
        {
            foreach(Delegate d in OnTurretBuild.GetInvocationList())
            {
                OnTurretBuild -= (EventHandler<BuildEventArgs>)d;
            }
        }
    }

    public void Initialize()
    {
        TotalCash = 0;

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

        AudioManager.Main.GetAudioTrack("Special").StopAllAudio();
        AudioManager.Main.SwitchMusicTracks("Special");
        AudioManager.Main.RequestMusic("Reward Song 2");
    }

    public void InitiateReward(int rewardValue)
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

        AudioManager.Main.SwitchMusicTracks("Special");


        var locked = FindObjectOfType<LockButton>().locked;

        if(!locked) GenerateOffer();

        GameManager.Main.SaveGame();
    }

    public void GenerateOffer()
    {
        RewardLevel _base = calculator.CalculateRewardLevel();
        RewardLevel _top = calculator.CalculateRewardLevel();
        
        foreach(WeaponBox weaponBox in weaponBoxes)
        {
            weaponBox.GenerateNewWeapon();
        }
        foreach(BaseBox baseBox in baseBoxes)
        {
            baseBox.GenerateNewBase();
        }
    }

    public void EarnCash(int amount)
    {
        EarnedCash = amount;
        cashTextAnimation.Play();
    }

    public void SpendCash(int amount)
    {
        SpendedCash = amount;
        cashTextAnimation.PlayReverse();
    }

    public void BuildSelection()
    {
        tutorialManager.TriggerPosBuildTutorial();
        buildBox.Clear();
        OnTurretBuild?.Invoke(this, new BuildEventArgs(ActiveSelection.GetComponent<TurretManager>()));
        turretConstructor.HandleBaseEffect(ActiveSelection);
        var manager = ActiveSelection.GetComponent<TurretManager>();
        SpendCash((int)manager.Stats[Stat.Cost]);
        ShipManager.Main.RegisterTurret(manager);


        AudioManager.Main.RequestGUIFX(buildSFX);

        ActiveSelection = null;
    }

    public void Exit()
    {
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

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();

        container.Add("totalCash", BitConverter.GetBytes(TotalCash));

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        // TotalCash = 0;
        // var cash = BitConverter.ToInt32(saveFile.GetValue("totalCash"));
        // EarnCash(cash);
    }
}

public class BuildEventArgs : EventArgs
{
    public TurretManager buildedTurret;

    public BuildEventArgs(TurretManager turret)
    {
        buildedTurret = turret;
    }
}
