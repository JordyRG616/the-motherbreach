using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;


public class WeaponBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject cachedWeapon;
    [SerializeField] private Image image;
    private BuildBox buildBox;
    private bool selected;
    private RectTransform statInfoBox;
    private BuildButton buildButton;

    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;
    [SerializeField] [FMODUnity.EventRef] private string returnSFX;
    private Material _material;
    private ParticleSystem activeVFX;
    private Light2D light2D;

    [Header("Light Colors")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color selectable;
    [SerializeField] private Color notSelectable;
    private UpgradeButton upgradeButton;
    

    void Start()
    {
        buildBox = FindObjectOfType<BuildBox>();
        activeVFX = GetComponentInChildren<ParticleSystem>();
        _material = new Material(GetComponent<Image>().material);
        GetComponent<Image>().material = _material;
        light2D = GetComponentInChildren<Light2D>();
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
        upgradeButton = FindObjectOfType<UpgradeButton>();
        // FindObjectOfType<InputManager>().OnSelectionClear += Unselect;
        buildButton = FindObjectOfType<BuildButton>();
    }

    public void GenerateNewWeapon()
    {
        cachedWeapon = TurretConstructor.Main.GetTop();
        var sprite = cachedWeapon.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
        image.color = Color.white;
        GetComponentsInChildren<RectTransform>()[1].localScale = new Vector2(1, 0);
        GetComponentInChildren<ExpandAnimation>().Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;

        if(cachedWeapon == null || RewardManager.Main.ActiveSelection)
        {
            AudioManager.Main.PlayInvalidSelection("");
            return;
        }
        if(buildBox.CheckCompability(cachedWeapon.GetComponent<Weapon>()) && !buildBox.CheckWeaponBox(this)) 
        {
            if(!buildBox.OnUpgrade)
            {
                activeVFX.Play();
                light2D.color = selectedColor;
                AudioManager.Main.RequestGUIFX(clickSFX);
                buildBox.ReceiveWeapon(cachedWeapon, this);
                selected = true;
                return;
            }
            else if(buildBox.OnUpgrade && IsSameWeapon())
            {
                activeVFX.Play();
                light2D.color = selectedColor;
                AudioManager.Main.RequestGUIFX(clickSFX);
                buildBox.ReceiveWeaponBox(this);
                buildBox.SetCostToWeaponCost(true);
                selected = true;

                return;
            }


        }
        else if(buildBox.CheckWeaponBox(this))
        {
            if(!buildBox.OnUpgrade)
            {
                AudioManager.Main.RequestGUIFX(returnSFX);
                buildBox.ClearWeapon(out cachedWeapon);
                image.color = Color.white;
                selected = false;
                return;
            }
            else
            {
                buildBox.ClearWeaponBox();
                buildButton.mode = BuildButton.ButtonMode.DONE;
                buildBox.UpdateStats();
            }
        } 
        AudioManager.Main.PlayInvalidSelection("");
    }

    private bool IsSameWeapon()
    {
        if(buildBox.selectedWeapon == null || cachedWeapon == null) return false;
        return buildBox.selectedWeapon.GetComponent<Weapon>().Id == cachedWeapon.GetComponent<Weapon>().Id;
    }

    void Update()
    {
        light2D.color = notSelectable;
        if(cachedWeapon == null) return;
        var check = buildBox.CheckCompability(cachedWeapon.GetComponent<Weapon>()) && cachedWeapon && !buildBox.OnUpgrade;
        var secondCheck = buildBox.OnUpgrade && IsSameWeapon();
        if(check || secondCheck) light2D.color = selectable;
    }

    public void Unselect()
    {
        activeVFX.Stop();
    }

    public void Unselect(object sender, EventArgs e)
    {
        activeVFX.Stop();
        var turret = upgradeButton.slot.occupyingTurret.GetComponent<TurretManager>();
    }

    public void Detach()
    {
        activeVFX.Stop();
        cachedWeapon = null;
        image.color = Color.clear;
    }

    public void Clear()
    {
        activeVFX.Stop();
        Destroy(cachedWeapon);
        image.color = Color.clear;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _material.SetInt("_Moving", 0);
        statInfoBox.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(cachedWeapon == null) return;
        _material.SetInt("_Moving", 1);
        AudioManager.Main.RequestGUIFX(hoverSFX);

        var text = "<size=150%><lowercase>" + cachedWeapon.name;

        if(!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(text, 200);
        }
    }
}
