using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuildBox : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image baseImage;
    
    [Header("Cost Text")]
    [SerializeField] private TextMeshProUGUI costText;

    [Header("Stats Texts")]
    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private TextMeshProUGUI restValue;

    [Header("Descriptions")]
    [SerializeField] private TextMeshProUGUI weaponEffect;
    [SerializeField] private TextMeshProUGUI baseEffect;
    

    private float weaponCost, baseCost;
    private float TotalCost
    {
        get
        {
            return weaponCost + baseCost;
        }
    }

    private WeaponBox selectedWeaponBox;
    private BaseBox selectedBaseBox;
    private GameObject selectedWeapon;
    private GameObject selectedBase;
    public bool OnUpgrade;

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
        costText.text = (OnUpgrade == true)? "-" : TotalCost + "$";
        if(selectedBase) baseEffect.text = selectedBase.GetComponent<BaseEffectTemplate>().DescriptionText();
        if(selectedWeapon)
        {
            healthValue.text = selectedWeapon.GetComponent<ActionController>().GetHealth().ToString();
            restValue.text = selectedWeapon.GetComponent<ActionEffect>().StatSet[Stat.Rest].ToString();
            weaponEffect.text = selectedWeapon.GetComponent<ActionEffect>().DescriptionText();
        }
    }

    
    public bool CheckCompability(BaseEffectTemplate testSubject)
    {
        if(!selectedWeapon) return true;
        var classes = selectedWeapon.GetComponent<ActionController>().GetClasses();
        foreach(WeaponClass _class in classes)
        {
            if(!testSubject.GetWeaponClasses().Contains(_class)) return false;
        }
        return true;
    }

    public bool CheckCompability(ActionController testSubject)
    {
        if(!selectedBase) return true;
        var classes = testSubject.GetComponent<ActionController>().GetClasses();
        foreach(WeaponClass _class in classes)
        {
            if(!selectedBase.GetComponent<BaseEffectTemplate>().GetWeaponClasses().Contains(_class)) return false;
        }
        return true;
    }

    private void ResetStats()
    {
        costText.text = "0$";
        baseEffect.text = "";
        healthValue.text = "";
        restValue.text = "";
        weaponEffect.text = "";
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
    }

    public void ClearWeapon()
    {
        if (selectedWeaponBox) selectedWeaponBox.Detach();
        selectedWeaponBox = null;
        selectedWeapon = null;
        weaponImage.color = Color.clear;
        ResetStats();
    }

    public void ClearWeapon(out GameObject _weapon)
    {
        if (selectedWeaponBox) selectedWeaponBox.Detach();
        selectedWeaponBox = null;
        _weapon = selectedWeapon;
        selectedWeapon = null;
        weaponImage.color = Color.clear;
        ResetStats();
    }

    public void ClearBase()
    {
        if (selectedBaseBox) selectedBaseBox.Detach();
        selectedBaseBox = null;
        selectedBase = null;
        baseImage.color = Color.clear;
    }

    public void ClearBase(out GameObject _base)
    {
        if (selectedBaseBox) selectedBaseBox.Detach();
        selectedBaseBox = null;
        _base = selectedBase;
        selectedBase = null;
        baseImage.color = Color.clear;
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
