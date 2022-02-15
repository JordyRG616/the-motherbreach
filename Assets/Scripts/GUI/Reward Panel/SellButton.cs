using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SellButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    private TextMeshProUGUI textMesh;
    private int refund;
    private UIAnimations cashTextAnimation;
    private RewardManager rewardManager;
    private TurretSlot cachedSlot;
    private InputManager inputManager;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string sellSFX;

    public event EventHandler OnTurretSell;

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
        Disable();
    }
    public void Disable()
    {
        if(cachedSlot ==  null) return;
        if(!gameObject.activeSelf) return;
        inputManager.OnSelectionClear -= Disable;
        gameObject.SetActive(false);
        cachedSlot = null;
    }
    public void SetButton(int refund, TurretSlot turretSlot)
    {
        textMesh.text = "sell" + " (" + refund +"$)";
        this.refund = refund;
        cachedSlot = turretSlot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(Sell());
        OnTurretSell?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator Sell()
    {
        var vfx = GetComponentInChildren<ParticleSystem>();
        vfx.Play();
        AudioManager.Main.RequestGUIFX(sellSFX);


        rewardManager.EarnedCash = refund;
        cashTextAnimation.Play();

        GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;

        cachedSlot.GetComponentInChildren<ParticleSystem>().Stop();
        ShipManager.Main.turrets.Remove(cachedSlot.occupyingTurret.GetComponent<TurretManager>());
        Destroy(cachedSlot.occupyingTurret);
        cachedSlot.Clear();

        FindObjectOfType<UpgradeButton>().gameObject.SetActive(false);
        
        yield return new WaitUntil(() => !vfx.IsAlive());

        GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
        GetComponent<Image>().enabled = true;

        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }

    void OnDisable()
    {
        inputManager.OnSelectionClear -= Disable;
    }
}
