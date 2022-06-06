using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StringHandler;

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
    [SerializeField] private TextMeshProUGUI weaponEffect;
    [SerializeField] private TextMeshProUGUI baseEffect;
    [SerializeField] private TextMeshProUGUI baseTrigger;

    [Header("Additional stats")]
    [SerializeField] private List<StatBox> additionalStatBoxes;
    [SerializeField] private TextMeshProUGUI _class;
    [SerializeField] private TextMeshProUGUI classDescription;



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


    void Start()
    {
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
        FindObjectOfType<SellButton>(true).OnTurretSell += Unselect;
        InputManager.Main.OnSelectionClear += Unselect;
    }

    public void ReceiveWeapon(GameObject receveidWeapon, WeaponBox box)
    {
        if(selectedWeaponBox) selectedWeaponBox.Unselect ();
        selectedWeapon = receveidWeapon;
        weaponImage.sprite = selectedWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.color = Color.white;
        weaponImage.GetComponent<UIAnimations>().Play();
        selectedWeaponBox = box;
        weaponCost = selectedWeapon.GetComponent<ActionController>().GetCost();
        UpdateStats();
    }

    public void ReceiveWeapon(GameObject receveidWeapon)
    {
        if(selectedWeaponBox) selectedWeaponBox.Unselect();
        selectedWeapon = receveidWeapon;
        weaponImage.sprite = selectedWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.color = Color.white;
        weaponImage.GetComponent<UIAnimations>().Play();
        weaponCost = selectedWeapon.GetComponent<ActionController>().GetCost();
        UpdateStats();
    }

    public void ReceiveWeaponBox(WeaponBox box)
    {
        selectedWeaponBox = box;
    }

    public void ReceiveBase(GameObject receveidBase, BaseBox box)
    {
        if(selectedBaseBox) selectedBaseBox.Unselect();
        selectedBase = receveidBase;
        baseImage.sprite = selectedBase.GetComponent<SpriteRenderer>().sprite;
        baseImage.color = Color.white;
        baseImage.GetComponent<UIAnimations>().Play();
        selectedBaseBox = box;
        baseCost = selectedBase.GetComponent<BaseEffectTemplate>().GetCost();
        UpdateStats();
    }

    public void ReceiveBase(GameObject receveidBase)
    {
        if(selectedBaseBox) selectedBaseBox.Unselect();
        selectedBase = receveidBase;
        baseImage.sprite = selectedBase.GetComponent<SpriteRenderer>().sprite;
        baseImage.color = Color.white;
        baseImage.GetComponent<UIAnimations>().Play();
        baseCost = selectedBase.GetComponent<BaseEffectTemplate>().GetCost();
        UpdateStats();
    }
    
    public void UpdateStats()
    {
        keywords.Clear();
        costText.text = (OnUpgrade)? "-" : TotalCost + "$";
        nameText.text = "";
        baseTrigger.text = "";
        if(selectedWeapon && selectedBase)
        {
            var _base = selectedBase.GetComponent<BaseEffectTemplate>();
            var _weapon = selectedWeapon.GetComponent<ActionController>();

            if(!OnUpgrade)
            {
                PreviewBaseEffect(selectedBase, selectedWeapon);
                nameText.text = "</uppercase>" + _base.name + " " + _weapon.name + "<lowercase> \n level <size=125%>0";
            }
            else
            {
                nameText.text = "</uppercase>" + selectedBase.name + " " + selectedWeapon.name + "<lowercase> \n level <size=125%>" + selectedWeapon.GetComponentInParent<TurretManager>().Level;
            }
        }
        if(selectedBase) 
        {
            var _base = selectedBase.GetComponent<BaseEffectTemplate>();
            Keyword baseKeyword;
            if(selectedWeapon)
            {
                var _weapon = selectedWeapon.GetComponent<ActionController>();
                baseEffect.text = _base.DescriptionText(out baseKeyword);
                if(_base.targetStats)
                {
                    var container = "";
                    foreach(Stat stat in _weapon.GetStatsOnShooters())
                    {
                        if(_base.StatIsTarget(stat))
                        {
                            container += _base.DescriptionTextByStat(stat) + "\n";
                        }
                    }

                    baseEffect.text = container;
                }
            }
            else baseEffect.text = _base.DescriptionText(out baseKeyword);
            baseTrigger.text = GetTriggerText(_base.GetTrigger());
            if(baseKeyword != Keyword.None) keywords.Add(baseKeyword);
        }
        if(selectedWeapon)
        {
            var effect = selectedWeapon.GetComponent<ActionEffect>();
            healthValue.text = selectedWeapon.GetComponent<ActionController>().GetHealth().ToString("0");
            restValue.text = effect.StatSet[Stat.Rest].ToString();
            weaponEffect.text = effect.DescriptionText(out var weaponKeyword);
            if(weaponKeyword != Keyword.None) keywords.Add(weaponKeyword);
            UpdateAdditionalStats(effect);
        }
    }

    public void SetCostToBaseCost(bool enabled)
    {
        if(enabled) costText.text = baseCost + "$";
        else costText.text = "-";
    }

    public void SetCostToWeaponCost(bool enabled)
    {
        if(enabled) costText.text = weaponCost + "$";
        else costText.text = "-";
    }

    public void PreviewBaseEffect(GameObject baseToPreview, GameObject weapon)
    {
        var _base = baseToPreview.GetComponent<BaseEffectTemplate>();
        var _weapon = weapon.GetComponent<ActionController>();

        if (_base.previewable)
        {
            _base.ReceiveWeapon(_weapon);
            _base.ApplyEffect();
        }
    }

    private void UpdateAdditionalStats(ActionEffect effect)
    {
        additionalStatBoxes[0].SetHeaderText(StatColorHandler.DamagePaint(Stat.Damage.ToString()) + ":");
        additionalStatBoxes[0].SetValueText(effect.StatSet[Stat.Damage].ToString());
        additionalStatBoxes[0].description = effect.damageText;

        additionalStatBoxes[1].SetHeaderText(StatColorHandler.StatPaint(effect.specializedStat.ToSplittedString()) + ":");
        var _txt = (effect.specializedStat == Stat.Efficiency) ? (effect.StatSet[effect.specializedStat] * 100).ToString() + "%" : effect.StatSet[effect.specializedStat].ToString();
        additionalStatBoxes[1].SetValueText(_txt);
        additionalStatBoxes[1].description = effect.specializedStatText;

        additionalStatBoxes[2].SetHeaderText(StatColorHandler.StatPaint(effect.secondaryStat.ToSplittedString()) + ":");
        _txt = (effect.secondaryStat == Stat.Efficiency) ? (effect.StatSet[effect.secondaryStat] * 100).ToString() + "%" : effect.StatSet[effect.secondaryStat].ToString();
        additionalStatBoxes[2].SetValueText(_txt);
        additionalStatBoxes[2].description = effect.secondaryStatText;

        string container = effect.weaponClass.ToSplittedString();
        
        _class.text = StatColorHandler.RestPaint(container);
        classDescription.text = DescriptionDictionary.Main.GetDescription(effect.weaponClass.ToString());
    }

    private string GetTriggerText(EffectTrigger baseEffectTrigger)
    {
        var container = string.Empty;
        switch(baseEffectTrigger)
        {
            case EffectTrigger.Immediate:
                container = "when constructed:";
            break;
            case EffectTrigger.EndOfWave:
                container = "at the end of each wave:";
            break;
            case EffectTrigger.StartOfWave:
                container = "at the start of each wave:";
            break;
            case EffectTrigger.OnDestruction:
                container = "when destructed:";
            break;
            case EffectTrigger.OnLevelUp:
                container = "when upgraded:";
            break;
            case EffectTrigger.OnHit:
                container = "when hit:";
            break;
            case EffectTrigger.OnEnemyDefeat:
                container = "when a enemy is defeated:";
            break;
            case EffectTrigger.OnTurretBuild:
                container = "when a turret is built:";
            break;
            case EffectTrigger.OnTurretSell:
                container = "when a turret is sold:";
            break;
            case EffectTrigger.Special:
                container = selectedBase.GetComponent<BaseEffectTemplate>().GetSpecialTrigger();
            break;
        }

        return container;
    }

    public bool CheckCompability(BaseEffectTemplate testSubject)
    {
        if(!testSubject.targetStats && !testSubject.targetTags) return true;
        if(!selectedWeapon) return true;
        var controller = selectedWeapon.GetComponent<ActionController>();
        var stats = controller.GetStatsOnShooters();
        foreach(Stat stat in stats)
        {
            if(testSubject.StatIsTarget(stat)) return true;
        }
        var tags = controller.GetShooters()[0].weaponClass;
        return testSubject.ContainsTag(tags);
    }

    public bool CheckCompability(ActionController testSubject)
    {
        if(!selectedBase) return true;
        var _base = selectedBase.GetComponent<BaseEffectTemplate>();
        if(!_base.targetStats && !_base.targetTags) return true;
        var stats = testSubject.GetStatsOnShooters();
        foreach(Stat stat in stats)
        {
            if(_base.StatIsTarget(stat)) return true;
        }
        var tags = testSubject.GetShooters()[0].weaponClass;
        return _base.ContainsTag(tags);
    }

    private void ResetStats()
    {
        costText.text = "0$";
        baseEffect.text = "";
        healthValue.text = "";
        restValue.text = "";
        weaponEffect.text = "";
        nameText.text = "";
        baseTrigger.text = "";

        foreach(StatBox box in additionalStatBoxes)
        {
            box.SetHeaderText("");
            box.SetValueText("");
            box.description = "";
        }

        _class.text = "";

        UpdateStats();
    }

    public (GameObject Weapon, GameObject Base) Selections()
    {
        return (selectedWeapon, selectedBase);
    }

    public void Clear()
    {
        ClearBase();
        ClearWeapon();
        OnUpgrade = false;
    }

    public void Unselect(object sender, EventArgs e)
    {
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
    }

    public void ClearWeapon()
    {
        if (selectedWeaponBox && !OnUpgrade) 
        {
            selectedWeapon.GetComponent<ActionController>().Reset();
            selectedWeaponBox.Detach();
        }
        selectedWeaponBox = null;
        selectedWeapon = null;
        weaponImage.color = Color.clear;
        weaponCost = 0;
        ResetStats();
    }

    public void ClearWeapon(out GameObject _weapon)
    {
        if (selectedWeaponBox && !OnUpgrade) 
        {
            selectedWeapon.GetComponent<ActionController>().Reset();
            selectedWeaponBox.Detach();
        }
        selectedWeaponBox = null;
        _weapon = selectedWeapon;
        selectedWeapon = null;
        weaponImage.color = Color.clear;
        weaponCost = 0;
        ResetStats();
    }

    public void ClearWeaponBox()
    {
        selectedWeaponBox.Unselect();
        selectedWeaponBox = null;
    }

    public void ClearBase()
    {
        if (selectedBaseBox) selectedBaseBox.Detach();
        if (selectedWeaponBox && !OnUpgrade) 
        {
            selectedWeapon.GetComponent<ActionController>().Reset();
            // selectedWeaponBox.Detach();
        }
        selectedBaseBox = null;
        selectedBase = null;
        baseImage.color = Color.clear;
        baseCost = 0;
        ResetStats();
    }

    public void ClearBase(out GameObject _base)
    {
        if (selectedBaseBox) selectedBaseBox.Detach();
        if (selectedWeaponBox && !OnUpgrade) 
        {
            selectedWeapon.GetComponent<ActionController>().Reset();
            // selectedWeaponBox.Detach();
        }
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

    public void ShowKeywordInfo()
    {
        if(keywords.Count == 0) return;
        var text = string.Empty;

        foreach(Keyword keyword in keywords)
        {
            text += KeywordHandler.KeywordDescription(keyword);
        }

        if(!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(text);
        }
    }

    public void HideKeywordInfo()
    {
        statInfoBox.gameObject.SetActive(false);
    }
}
