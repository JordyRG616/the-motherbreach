using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite clickSprite;
    [SerializeField] private TextMeshProUGUI textMesh;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string upgradeSFX;
    private Sprite ogSprite;
    private Image image;
    private InputManager inputManager;
    private TurretSlot clickedSlot;
    public TurretSlot slot
    {
        get 
        {
            return clickedSlot;
        }
    }
    private BuildBox buildBox;
    private BuildButton buildButton;
    public bool onUpgrade {get; private set;}
    private RectTransform infoBox;


    void Awake()
    {
        image = GetComponent<Image>();
        ogSprite = image.sprite;

        buildBox = FindObjectOfType<BuildBox>();
        buildButton = FindObjectOfType<BuildButton>();

        inputManager = InputManager.Main;

        infoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        inputManager.OnSelectionClear += Disable;
    }

    public void SetButton(TurretSlot slot)
    {
        Disable();
        gameObject.SetActive(true);
        clickedSlot = slot;
        textMesh.text = "MODIFY";
    }

    public void Disable(object sender, EventArgs e)
    {
        if(clickedSlot == null) return;
        clickedSlot.GetComponentInChildren<ParticleSystem>().Stop();
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
        clickedSlot = null;
        if(onUpgrade) 
        {
            buildBox.Clear(); 
            buildBox.OnUpgrade = false;
            buildButton.mode = BuildButton.ButtonMode.BUILD;
        }
        onUpgrade = false;
    }

    public void Disable()
    {
        if(clickedSlot == null) return;
        clickedSlot.GetComponentInChildren<ParticleSystem>().Stop();
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
        clickedSlot = null;
        if(onUpgrade) 
        {
            buildBox.Clear(); 
            buildBox.OnUpgrade = false;
            buildButton.mode = BuildButton.ButtonMode.BUILD;
        }
        onUpgrade = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        
        if(!onUpgrade)
        {
            SetUpgradeOption();
            SetUpgradeText();
        }
        else
        {
            Upgrade();
        }
    }

    private void Upgrade()
    {
        image.sprite = clickSprite;

        var turretManager = clickedSlot.occupyingTurret.GetComponent<TurretManager>();
        var cost = turretManager.Level + 1;

        if(RewardManager.Main.TotalCash >= cost && turretManager.Level < turretManager.maxLevel)
        {
            GetComponentInChildren<ParticleSystem>().Play();
            AudioManager.Main.RequestGUIFX(upgradeSFX);
            RewardManager.Main.SpendCash(cost);
            turretManager.LevelUp();
            buildBox.UpdateStats();
            cost = turretManager.Level + 1;
            SetButtonText(cost);
            SetUpgradeText();
            return;
        }
        else
        {
            if(turretManager.Level >= turretManager.maxLevel) AudioManager.Main.PlayInvalidSelection("Turret at maximum level");
            else AudioManager.Main.PlayInvalidSelection("Not enough cash");
        }
    }

    private void SetButtonText(int cost)
    {
        if(cost < 6) textMesh.text = "upgrade (" + cost + "$)";
        else 
        {
            textMesh.text = "max. level";
            infoBox.gameObject.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = ogSprite;
    }

    private void SetUpgradeOption()
    {
        image.sprite = clickSprite;

        var cost = clickedSlot.occupyingTurret.GetComponent<TurretManager>().Level + 1;

        SetButtonText(cost);

        FindObjectOfType<SellButton>().SetButton(0, clickedSlot, SellButton.ButtonMode.Replace);
        buildButton.mode = BuildButton.ButtonMode.DONE;

        onUpgrade = true;
    }

    void OnDisable()
    {
        inputManager.OnSelectionClear -= Disable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);

        if (!onUpgrade) return;

        SetUpgradeText();
    }

    private void SetUpgradeText()
    {
        var action = clickedSlot.occupyingTurret.GetComponent<TurretManager>().actionController;
        var level = clickedSlot.occupyingTurret.GetComponent<TurretManager>().Level;
        if(level >= 5) return;
        var text = action.GetShooters()[0].upgradeText(level + 1);
        text = "<size=125%>" + text;

        infoBox.gameObject.SetActive(true);
        infoBox.GetComponent<StatInfoBox>().SetText(text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.gameObject.SetActive(false);

    }
}
