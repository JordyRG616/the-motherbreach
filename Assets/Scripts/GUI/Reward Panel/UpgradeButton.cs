using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Sprite clickSprite;
    [SerializeField] private TextMeshProUGUI textMesh;
    private Sprite ogSprite;
    private Image image;
    private InputManager inputManager;
    private TurretSlot clickedSlot;
    private BuildBox buildBox;
    private bool onUpgrade;


    void Awake()
    {
        image = GetComponent<Image>();
        ogSprite = image.sprite;

        buildBox = FindObjectOfType<BuildBox>();

        inputManager = InputManager.Main;
    }

    void OnEnable()
    {
        inputManager.OnSelectionClear += Disable;
    }

    public void SetButton(TurretSlot slot)
    {
        clickedSlot = slot;
        textMesh.text = "MODIFY";
    }

    public void Disable(object sender, EventArgs e)
    {
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
        if(onUpgrade) buildBox.Clear();
    }

    public void Disable()
    {
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
        if(onUpgrade) buildBox.Clear();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!onUpgrade)
        {
            SendToBuildBox();
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

        if(RewardManager.Main.TotalCash >= cost && turretManager.Level < 5) 
        {
            RewardManager.Main.SpendCash(cost);
            turretManager.LevelUp();
            textMesh.text = "upgrade (" + cost + "$)";
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = ogSprite;
    }

    private void SendToBuildBox()
    {
        image.sprite = clickSprite;
        
        var _weapon = clickedSlot.occupyingTurret.GetComponentInChildren<ActionController>().gameObject;
        var _base = clickedSlot.occupyingTurret.GetComponentInChildren<BaseEffectTemplate>().gameObject;

        buildBox.ReceiveWeapon(_weapon);
        buildBox.ReceiveBase(_base);

        FindObjectOfType<SellButton>().Disable();
        FindObjectOfType<BuildButton>().mode = BuildButton.ButtonMode.DONE;

        var cost = clickedSlot.occupyingTurret.GetComponent<TurretManager>().Level + 1;

        textMesh.text = "UPGRADE (" + cost + "$)";

        onUpgrade = true;
    }
}
