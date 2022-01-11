using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellButton : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textMesh;
    private int refund;
    private UIAnimations cashTextAnimation;
    private RewardManager rewardManager;
    private TurretSlot cachedSlot;
    private InputManager inputManager;

    void Awake()
    {
        textMesh = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        cashTextAnimation = FindObjectOfType<CashTextAnimation>();
        rewardManager = RewardManager.Main;
        inputManager = InputManager.Main;
    }

    void OnEnable()
    {
        inputManager.OnSelectionClear += Disable;
    }

    public void Disable(object sender, EventArgs e)
    {
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
    }
    public void Disable()
    {
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
    }
    public void SetButton(int refund, TurretSlot turretSlot)
    {
        textMesh.text = "sell" + " (" + refund +"$)";
        this.refund = refund;
        cachedSlot = turretSlot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        rewardManager.EarnedCash = refund;
        cashTextAnimation.Play();

        Destroy(cachedSlot.occupyingTurret);
        cachedSlot.Clear();

        gameObject.SetActive(false);
        FindObjectOfType<UpgradeButton>().gameObject.SetActive(false);
    }
}
