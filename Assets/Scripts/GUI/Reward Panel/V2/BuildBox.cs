using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StringHandler;
using System.Linq;

public class BuildBox : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image baseImage;
    
    [Header("Name text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [Header("Cost Text")]
    [SerializeField] private TextMeshProUGUI costText;

    [Header("Stats Texts")]
    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private TextMeshProUGUI restValue;

    [Header("Descriptions")]
    [SerializeField] private TextMeshProUGUI weaponDescription;
    [SerializeField] private TextMeshProUGUI upgradePoints;

    [Header("Additional stats")]
    [SerializeField] private List<StatBox> statBoxes;
    [SerializeField] private TextMeshProUGUI _class;
    [SerializeField] private TextMeshProUGUI classDescription;

    [Header("Programs")]
    [SerializeField] private List<ProgramBox> programBoxes;

    [HideInInspector] public float weaponCost, baseCost;
    public float TotalCost
    {
        get
        {
            return weaponCost + baseCost;
        }
    }

    public WeaponBox selectedWeaponBox {get; private set;}
    public BaseBox selectedBaseBox {get; private set;}
    public GameObject selectedWeapon {get; private set;}
    public GameObject selectedBase {get; private set;}
    public GameObject baseToReplace;
    private List<Keyword> keywords = new List<Keyword>();
    private RectTransform statInfoBox;
    public bool OnUpgrade;
    private BuildButton buildButton;

    public void ActivateUpgradeMode()
    {
        OnUpgrade = true;
        buildButton.mode = BuildButton.ButtonMode.UPGRADE;

    }

    public void DeactivateUpgradeMode()
    {
        OnUpgrade = false;
        buildButton.mode = BuildButton.ButtonMode.BUILD;
    }

    void Start()
    {
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
        buildButton = FindObjectOfType<BuildButton>();
        FindObjectOfType<SellButton>(true).OnTurretSell += Unselect;
        InputManager.Main.OnSelectionClear += Unselect;
    }

    public void ReceiveWeapon(GameObject receveidWeapon, WeaponBox box)
    {
        if(selectedWeaponBox)
        {
            UndoFoundationEffect();
            selectedWeaponBox.Unselect ();
        }
        selectedWeapon = receveidWeapon;
        weaponImage.sprite = selectedWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.color = Color.white;
        weaponImage.GetComponent<UIAnimations>().Play();
        selectedWeaponBox = box;
        weaponCost = selectedWeapon.GetComponent<Weapon>().Cost;
        UpdateStats();
    }

    public void ReceiveWeapon(GameObject receveidWeapon)
    {
        if(selectedWeaponBox)
        {
            UndoFoundationEffect();
            selectedWeaponBox.Unselect();
        }
        selectedWeapon = receveidWeapon;
        weaponImage.sprite = selectedWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.color = Color.white;
        weaponImage.GetComponent<UIAnimations>().Play();
        weaponCost = selectedWeapon.GetComponent<Weapon>().Cost;
        UpdateStats();
    }

    public void ReceiveWeaponBox(WeaponBox box)
    {
        ClearWeaponBox();
        ClearBaseBox();
        selectedWeaponBox = box;
    }

    public void ReceiveBaseBox(BaseBox box)
    {
        ClearWeaponBox();
        ClearBaseBox();
        selectedBaseBox = box;
    }

    public void ReceiveBase(GameObject receveidBase, BaseBox box)
    {
        if(selectedBaseBox)
        {
            UndoFoundationEffect();
            selectedBaseBox.Unselect();
        }
        selectedBase = receveidBase;
        baseImage.sprite = selectedBase.GetComponent<SpriteRenderer>().sprite;
        baseImage.color = Color.white;
        baseImage.GetComponent<UIAnimations>().Play();
        selectedBaseBox = box;
        baseCost = selectedBase.GetComponent<Foundation>().Cost;
        UpdateStats();
    }

    public void ReceiveBase(GameObject receveidBase)
    {
        if (selectedBaseBox)
        {
            UndoFoundationEffect();
            selectedBaseBox.Unselect();
        }
        selectedBase = receveidBase;
        baseImage.sprite = selectedBase.GetComponent<SpriteRenderer>().sprite;
        baseImage.color = Color.white;
        baseImage.GetComponent<UIAnimations>().Play();
        baseCost = selectedBase.GetComponent<Foundation>().Cost;
        UpdateStats();
    }
    
    public void UpdateStats()
    {
        keywords.Clear();
        costText.text = (OnUpgrade)? "-" : TotalCost + "$";
        nameText.text = "";
        if(selectedWeapon && selectedBase)
        {
            var _base = selectedBase.GetComponent<Foundation>();
            var _weapon = selectedWeapon.GetComponent<Weapon>();

            weaponDescription.text = _weapon.Description;

            if(!OnUpgrade)
            {
                nameText.text = _base.name + " " + _weapon.name + "\n level 0";
            }
            else
            {
                nameText.text = selectedBase.name + " " + selectedWeapon.name + "\n level " + selectedWeapon.GetComponentInParent<TurretManager>().Level;
                upgradePoints.text = _weapon.GetComponentInParent<TurretManager>().upgradePoints + " upgrade point(s) available";
            }
        }
        if(selectedWeapon)
        {
            var weapon = selectedWeapon.GetComponent<Weapon>();
            List<Trait> _programs = new List<Trait>();

            foreach(Trait program in weapon.InitialPrograms)
            {
                _programs.Add(program);
            }
            
            if(selectedBase) 
            {
                var foundation = selectedBase.GetComponent<Foundation>();

                foreach(TurretStat stat in foundation.exposedStats)
                {
                    if (weapon.HasDormentStat(stat)) weapon.ExposeDormentStat(stat);
                }

                foreach (Trait program in foundation.Programs)
                {
                    _programs.Add(program);
                }
            }

            UpdateAdditionalStats(weapon);
            UpdateProgramBoxes(_programs);
        }
        if (selectedBase && !selectedWeapon)
        {
            var foundation = selectedBase.GetComponent<Foundation>();
            List<Trait> _programs = new List<Trait>();

            foreach (Trait program in foundation.Programs)
            {
                _programs.Add(program);
            }

            UpdateProgramBoxes(_programs);
        }
    }

    public void SetCostToBaseCost(bool enabled)
    {
        if(enabled) costText.text = GetUpgradeCost() + "$";
        else costText.text = "-";
    }

    public void SetCostToWeaponCost(bool enabled)
    {
        if(enabled) costText.text = GetUpgradeCost() + "$";
        else costText.text = "-";
    }

    public float GetUpgradeCost()
    {
        if (!OnUpgrade) return 0;
        if (selectedWeaponBox) return Mathf.Ceil(weaponCost / 2);
        if (selectedBaseBox) return Mathf.Ceil(baseCost / 2);
        return 0;
    }

    public void UpgradeTurret()
    {
        if(selectedWeaponBox)
        {
            selectedWeapon.GetComponentInParent<TurretManager>().LevelUp();
            selectedWeaponBox.Clear();
            ClearWeaponBox();
        }
        if (selectedBaseBox)
        {
            selectedWeapon.GetComponentInParent<TurretManager>().LevelUp();
            selectedBaseBox.Clear();
            ClearBaseBox();
        }
        UpdateStats();
    }

    private void UpdateAdditionalStats(Weapon weapon)
    {
        var stats = weapon.GetAllStats();
        stats = stats.OrderBy(x => x.sortingIndex).ToList();

        for (int i = 0; i < stats.Count; i++)
        {
            var statbox = statBoxes[i];
            var stat = stats[i];

            if (weapon.HasStat(stat)) statbox.Activate();
            else statbox.Deactivate();

            statbox.SetHeaderText(stat.publicName);
            var desc = (stat.Initiated) ? stat.GetLiteralValue() : stat.GetLiteralStartingValue();
            statbox.SetValueText(desc);
            statbox.description = stat.statDescription;
            if (stat.upgradeCost > 1) statbox.description += StatColorHandler.DamagePaint("\nrequires " + stat.upgradeCost + " points!");
            statbox.ReceiveStat(stat);
        }
    }

    private void UpdateProgramBoxes(List<Trait> programs)
    {
        var lockedLevel = 1;
        var component = (selectedBase == null) ? selectedWeapon : selectedBase;
        var manager = component.GetComponentInParent<TurretManager>();

        for(int i = 0; i < programBoxes.Count; i++)
        {
            var box = programBoxes[i];
            var program = (i >= programs.Count) ? null : programs[i];

            if(program != null)
            {
                box.SetupFilledBox(program);
                lockedLevel++;
            } else if(manager != null && manager.Level >= lockedLevel * 3)
            {
                box.SetupAvailableBox();
                lockedLevel++;
            } else
            {
                box.SetupLockedBox(lockedLevel * 3);
                lockedLevel++;
            }
        }
    }

    public bool CheckCompability(Foundation testedFoundation)
    {
        if (!selectedWeapon) return true;
        if (testedFoundation.exposedStats.Count == 0) return true;
        var weapon = selectedWeapon.GetComponent<Weapon>();

        foreach(TurretStat stat in testedFoundation.exposedStats)
        {
            if (weapon.HasDormentStat(stat)) return true;
        }

        return false;
    }

    public bool CheckCompability(Weapon testedWeapon)
    {
        if (!selectedBase) return true;
        var foundation = selectedBase.GetComponent<Foundation>();
        if (foundation.exposedStats.Count == 0) return true;

        foreach (TurretStat stat in foundation.exposedStats)
        {
            if (testedWeapon.HasDormentStat(stat)) return true;
        }

        return false;
    }

    private void ResetStats()
    {
        costText.text = "0$";
        healthValue.text = "";
        restValue.text = "";
        nameText.text = "";
        weaponDescription.text = "";
        upgradePoints.text = "";

        foreach(StatBox box in statBoxes)
        {
            box.SetHeaderText("");
            box.SetValueText("");
            box.description = "";
            box.ReceiveStat(null);
            box.Deactivate();
        }

        programBoxes.ForEach(x => x.EmptyBox());

        _class.text = "";

        UpdateStats();
    }

    public (GameObject Weapon, GameObject Base) Selections()
    {
        return (selectedWeapon, selectedBase);
    }

    private void UndoFoundationEffect()
    {
        if (selectedWeapon && selectedBase)
        {
            var weapon = selectedWeapon.GetComponent<Weapon>();
            var foundation = selectedBase.GetComponent<Foundation>();

            if (weapon.GetComponentInParent<TurretManager>() != null) return;

            foreach (TurretStat stat in foundation.exposedStats)
            {
                    weapon.HideExposedStat(stat);
            }
        }
    }

    public void Clear()
    {
        ClearBase();
        ClearWeapon();
        DeactivateUpgradeMode();
    }

    public void Unselect(object sender, EventArgs e)
    {
        UndoFoundationEffect();
        if(selectedWeaponBox) 
        {
            selectedWeaponBox.Unselect();
            selectedWeaponBox = null;
        }
        if(selectedBaseBox) 
        {
            selectedBaseBox.Unselect();
            selectedBaseBox = null;
        }
        Clear();
        DeactivateUpgradeMode();
    }

    public void ClearWeapon()
    {
        UndoFoundationEffect();
        if (selectedWeaponBox) selectedWeaponBox.Detach();
        selectedWeaponBox = null;
        selectedWeapon = null;
        weaponImage.color = Color.clear;
        weaponCost = 0;
        ResetStats();
    }

    public void ClearWeapon(out GameObject _weapon)
    {
        UndoFoundationEffect();
        if (selectedWeaponBox) selectedWeaponBox.Detach();
        selectedWeaponBox = null;
        _weapon = selectedWeapon;
        selectedWeapon = null;
        weaponImage.color = Color.clear;
        weaponCost = 0;
        ResetStats();
    }

    public void ClearWeaponBox()
    {
        if(selectedWeaponBox) selectedWeaponBox.Unselect();
        selectedWeaponBox = null;
    }

    public void ClearBaseBox()
    {
        if(selectedBaseBox) selectedBaseBox.Unselect();
        selectedBaseBox = null;
    }

    public void ClearBase()
    {
        UndoFoundationEffect();
        if (selectedBaseBox) selectedBaseBox.Detach();
        selectedBaseBox = null;
        selectedBase = null;
        baseImage.color = Color.clear;
        baseCost = 0;
        ResetStats();
    }

    public void ClearBase(out GameObject _base)
    {
        UndoFoundationEffect();
        if (selectedBaseBox) selectedBaseBox.Detach();
        selectedBaseBox = null;
        _base = selectedBase;
        selectedBase = null;
        baseImage.color = Color.clear;
        baseCost = 0;
        ResetStats();
    }

    public bool CheckWeaponBox(WeaponBox checkTarget)
    {
        return selectedWeaponBox == checkTarget;
    }

    public bool CheckBaseBox(BaseBox checkTarget)
    {
        return selectedBaseBox == checkTarget;
    }
}
