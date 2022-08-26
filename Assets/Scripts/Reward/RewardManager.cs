using System;
using System.Linq;
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
    private UIAnimationManager animationManager;
    [HideInInspector] public int TotalCash, EarnedCash, SpendedCash;
    private BuildBox buildBox;
    private TutorialManager tutorialManager;

    public GameObject ActiveSelection {get; private set;}

    public event EventHandler OnRewardSelection;
    public event EventHandler<BuildEventArgs> OnTurretBuild;

    private UIAnimations rewardPanelAnimation;

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
        animationManager = GameObject.FindGameObjectWithTag("RewardAnimation").GetComponent<UIAnimationManager>();

        buildBox = FindObjectOfType<BuildBox>();
        tutorialManager = FindObjectOfType<TutorialManager>();

        inputManager = InputManager.Main;
        inputManager.OnSelectionClear += ClearSelection;

        weaponBoxes = FindObjectsOfType<WeaponBox>(true).ToList();
        baseBoxes = FindObjectsOfType<BaseBox>(true).ToList();
        cashTextAnimation = FindObjectOfType<CashTextAnimation>();
        rewardPanelAnimation = GameObject.FindGameObjectWithTag("Build Box").GetComponent<UIAnimations>();

        AudioManager.Main.GetAudioTrack("Special").StopAllAudio();
        AudioManager.Main.SwitchMusicTracks("Special");
        AudioManager.Main.RequestMusic("Reward Song 2");
    }

    public void InitiateReward(int rewardValue, bool firstOff)
    {
        SpendedCash = 0;
        EarnCash(rewardValue);
        ShipManager.Main.transform.rotation = Quaternion.identity;
        var ship = FindObjectOfType<ShipController>();
        ship.StopFX();
        ship.transform.rotation = Quaternion.identity;
        ship.GetComponent<Rigidbody2D>().Sleep();
        animationManager.Play();
        
        GenerateOffer();
        if (firstOff && DataManager.Main.SaveFileExists()) LoadBoxes(DataManager.Main.saveFile);

        GameManager.Main.SaveGame();
    }

    public void GenerateOffer()
    {
        foreach(WeaponBox weaponBox in weaponBoxes)
        {
            weaponBox.GenerateNewWeapon();
        }
        foreach(BaseBox baseBox in baseBoxes)
        {
            baseBox.GenerateNewBase();
        }
    }

    public void GenerateWeaponOffer()
    {
        foreach (WeaponBox weaponBox in weaponBoxes)
        {
            weaponBox.GenerateNewWeapon();
        }
    }

    public void GenerateFoundationOffer()
    {
        foreach (BaseBox baseBox in baseBoxes)
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
        SpendCash((int)buildBox.TotalCost);
        tutorialManager.TriggerPosBuildTutorial();
        buildBox.Clear();
        OnTurretBuild?.Invoke(this, new BuildEventArgs(ActiveSelection.GetComponent<TurretManager>()));
        var manager = ActiveSelection.GetComponent<TurretManager>();
        ShipManager.Main.RegisterTurret(manager);

        AudioManager.Main.RequestGUIFX(buildSFX);

        rewardPanelAnimation.PlayReverse();
        ActiveSelection = null;
    }

    public void Exit()
    {
        EliminateOffer();
        
        AudioManager.Main.GetAudioTrack("SFX").UnpauseAudio();
        AudioManager.Main.SwitchMusicTracks("Music");
        
        OnRewardSelection?.Invoke(this, EventArgs.Empty);
    }

    public void EliminateOffer()
    {
        buildBox.Unselect(this, EventArgs.Empty);

        foreach (WeaponBox weaponBox in weaponBoxes)
        {
            weaponBox.Clear();
        }
        foreach(BaseBox baseBox in baseBoxes)
        {
            baseBox.Clear();
        }
    }

    public void EliminateWeaponOffer()
    {
        buildBox.Unselect(this, EventArgs.Empty);
        foreach (WeaponBox weaponBox in weaponBoxes)
        {
            weaponBox.Clear();
        }
    }

    public void EliminateFoundationOffer()
    {
        buildBox.Unselect(this, EventArgs.Empty);
        foreach (BaseBox baseBox in baseBoxes)
        {
            baseBox.Clear();
        }
    }

    public void SetSelection(GameObject _weapon, GameObject _base)
    {
        ActiveSelection = turretConstructor.Construct(_weapon, _base);
        ActiveSelection.SetActive(true);
        ActiveSelection.transform.position = buildBox.transform.position;
        ActiveSelection.AddComponent<TrackingDevice>().StartTracking();
        foreach (SpriteRenderer renderer in ActiveSelection.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = Color.white;
        }
        foreach(TurretVFXManager vfx in ActiveSelection.GetComponentsInChildren<TurretVFXManager>())
        {
            vfx.EnableSelected();
        }

        rewardPanelAnimation.Play();
    }

    private void ClearSelection(object sender, EventArgs e)
    {
        if(ActiveSelection == null) return;
        ActiveSelection.GetComponentInChildren<Weapon>().gameObject.SetActive(false);
        ActiveSelection.GetComponentInChildren<Weapon>(true).transform.parent = null;
        ActiveSelection.GetComponentInChildren<Foundation>().gameObject.SetActive(false);
        ActiveSelection.GetComponentInChildren<Foundation>(true).transform.parent = null;
        rewardPanelAnimation.PlayReverse();
        Destroy(ActiveSelection);
    }

    public void RerrollAll()
    {
        EliminateOffer();
        GenerateOffer();
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();

        container.Add("totalCash", BitConverter.GetBytes(TotalCash));

        for (int i = 0; i < weaponBoxes.Count; i++)
        {
            var key = "weaponBox" + i;
            var value = BitConverter.GetBytes(weaponBoxes[i].Occupied);
            container.Add(key, value);
        }

        for (int i = 0; i < baseBoxes.Count; i++)
        {
            var key = "baseBox" + i;
            var value = BitConverter.GetBytes(baseBoxes[i].Occupied);
            container.Add(key, value);
        }

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
    }

    public void LoadBoxes(SaveFile saveFile)
    {
        for (int i = 0; i < weaponBoxes.Count; i++)
        {
            var key = "weaponBox" + i;
            if (saveFile.ContainsSavedContent(key))
            {
                var _occupied = BitConverter.ToBoolean(saveFile.GetValue(key));
                if (!_occupied) weaponBoxes[i].Clear();
            }
        }

        for (int i = 0; i < baseBoxes.Count; i++)
        {
            var key = "baseBox" + i;
            if (saveFile.ContainsSavedContent(key))
            {
                var _occupied = BitConverter.ToBoolean(saveFile.GetValue(key));
                if (!_occupied) baseBoxes[i].Clear();
            }
        }
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
