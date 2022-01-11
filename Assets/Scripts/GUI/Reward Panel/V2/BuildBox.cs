using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void ReceiveWeapon(GameObject receveidWeapon, WeaponBox box)
    {
        selectedWeapon = receveidWeapon;
        weaponImage.sprite = selectedWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.color = Color.white;
        selectedWeaponBox = box;
        weaponCost = selectedWeapon.GetComponent<ActionController>().GetCost();
        UpdateStats();
    }

    public void ReceiveWeapon(GameObject receveidWeapon)
    {
        selectedWeapon = receveidWeapon;
        weaponImage.sprite = selectedWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.color = Color.white;
        weaponCost = selectedWeapon.GetComponent<ActionController>().GetCost();
        UpdateStats();
    }

    public void ReceiveBase(GameObject receveidBase, BaseBox box)
    {
        selectedBase = receveidBase;
        baseImage.sprite = selectedBase.GetComponent<SpriteRenderer>().sprite;
        baseImage.color = Color.white;
        selectedBaseBox = box;
        baseCost = selectedBase.GetComponent<BaseEffectTemplate>().GetCost();
        UpdateStats();
    }

    public void ReceiveBase(GameObject receveidBase)
    {
        selectedBase = receveidBase;
        baseImage.sprite = selectedBase.GetComponent<SpriteRenderer>().sprite;
        baseImage.color = Color.white;
        baseCost = selectedBase.GetComponent<BaseEffectTemplate>().GetCost();
        UpdateStats();
    }
    
    private void UpdateStats()
    {
        costText.text = TotalCost + "$";
        if(selectedBase) baseEffect.text = selectedBase.GetComponent<BaseEffectTemplate>().DescriptionText();
        if(selectedWeapon)
        {
            healthValue.text = selectedWeapon.GetComponent<ActionController>().GetHealth().ToString();
            restValue.text = selectedWeapon.GetComponent<ActionEffect>().StatSet[Stat.Rest].ToString();
            weaponEffect.text = selectedWeapon.GetComponent<ActionEffect>().DescriptionText();
        }
    }

    private void ResetStats()
    {
        costText.text = "0$";
        baseEffect.text = "";
        healthValue.text = "";
        restValue.text = "";
        weaponEffect.text = "";
    }

    public (GameObject Weapon, GameObject Base) Selections()
    {
        return (selectedWeapon, selectedBase);
    }

    public void Clear()
    {
        ResetStats();
        selectedBaseBox.Detach();
        selectedWeaponBox.Detach();
        selectedWeapon = null;
        selectedBase = null;
        weaponImage.color = Color.clear;
        baseImage.color = Color.clear;
    }

}
