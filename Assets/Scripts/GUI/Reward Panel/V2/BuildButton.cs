using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class BuildButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public enum ButtonMode {BUILD, DONE, UPGRADE};


    [SerializeField] private Sprite clickedSprite;
    private Sprite ogSprite;
    private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;
    private RewardManager rewardManager;
    private BuildBox buildBox;
    private UpgradeButton upgradeButton;
    [HideInInspector] public ButtonMode mode = ButtonMode.BUILD;
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;




    void Start()
    {
        textMesh = image.GetComponentInChildren<TextMeshProUGUI>();
        ogSprite = image.sprite;
        buildBox = FindObjectOfType<BuildBox>();
        rewardManager = RewardManager.Main;
        upgradeButton = FindObjectOfType<UpgradeButton>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        
        image.sprite = clickedSprite;
        // textMesh.color = Color.white;
        if(mode == ButtonMode.BUILD) Build();
        if (mode == ButtonMode.DONE) Done();
        if (mode == ButtonMode.UPGRADE) Upgrade();
    }

    private void Upgrade()
    {
        if(rewardManager.TotalCash >= buildBox.weaponCost)
            {
                AudioManager.Main.RequestGUIFX(clickSFX);
                upgradeButton.Upgrade(buildBox.weaponCost);
            }
            else AudioManager.Main.PlayInvalidSelection("Not enough cash");
    }

    private void Done()
    {
        AudioManager.Main.RequestGUIFX(clickSFX);
        if(buildBox.baseToReplace)
        {
            FindObjectOfType<SellButton>(true).Replace();
        }
        upgradeButton.Disable();
        // FindObjectOfType<SellButton>().Disable();
    }

    private void Build()
    {
        if (buildBox.Selections().Weapon != null && buildBox.Selections().Base != null && rewardManager.ActiveSelection == null)
        {
            if(rewardManager.TotalCash >= buildBox.TotalCost)
            {
                AudioManager.Main.RequestGUIFX(clickSFX);
                rewardManager.SetSelection(buildBox.Selections().Weapon, buildBox.Selections().Base);
            }
            else AudioManager.Main.PlayInvalidSelection("Not enough cash");
        }
        else
        {
            if(buildBox.Selections().Weapon == null && buildBox.Selections().Base == null)
            {
                AudioManager.Main.PlayInvalidSelection("Select a weapon and a base");
                return;
            }
            if(buildBox.Selections().Weapon == null) 
            {
                AudioManager.Main.PlayInvalidSelection("Select a weapon first");
                return;
            }
            if(buildBox.Selections().Base == null)
            {
                AudioManager.Main.PlayInvalidSelection("Select a base first");
                return;
            } 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = ogSprite;
        // textMesh.color = Color.black;
    }

    void Update()
    {
        if(buildBox.baseToReplace)
        {
            textMesh.text = "replace";
            return;
        }
        textMesh.text = mode.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }
}
