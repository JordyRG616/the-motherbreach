using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SellButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public enum ButtonMode {Sell, Replace}
    private ButtonMode mode = ButtonMode.Sell;

    private TextMeshProUGUI textMesh;
    private int refund;
    private UIAnimations cashTextAnimation;
    private RewardManager rewardManager;
    private BuildBox buildBox;
    private TurretSlot cachedSlot;
    private InputManager inputManager;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string sellSFX;
    [SerializeField] [FMODUnity.EventRef] private string replaceSFX;


    public event EventHandler OnTurretSell;

    void Awake()
    {
        textMesh = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        cashTextAnimation = FindObjectOfType<CashTextAnimation>();
        rewardManager = RewardManager.Main;
        inputManager = InputManager.Main;
        buildBox = FindObjectOfType<BuildBox>();
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
    public void SetButton(int refund, TurretSlot turretSlot, ButtonMode mode = ButtonMode.Sell)
    {
        if(mode == ButtonMode.Sell) textMesh.text = "sell" + " (" + refund +"$)";
        else textMesh.text = "<size=70%>replace";

        this.mode = mode;
        this.refund = refund;
        cachedSlot = turretSlot;
        if(mode == ButtonMode.Replace) gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        
        if(mode == ButtonMode.Sell)
        {
            StartCoroutine(Sell());
            OnTurretSell?.Invoke(this, EventArgs.Empty);
        }
        // else
        // {
        //     Replace();
        // }
    }

    public void Replace()
    {
        var _base = buildBox.baseToReplace;
        if (_base == null) 
        {
            AudioManager.Main.PlayInvalidSelection("Select a base to replace");
            return;
        }
        var cost = _base.GetComponent<BaseEffectTemplate>().GetCost();
        if (rewardManager.TotalCash >= cost)
        {
            AudioManager.Main.RequestGUIFX(replaceSFX);
            TurretConstructor.Main.ReplaceBase(cachedSlot.occupyingTurret, _base);
            rewardManager.SpendCash((int)cost);
            buildBox.selectedBaseBox.Detach();
            buildBox.UpdateStats();
            buildBox.baseToReplace = null;
        }
        else AudioManager.Main.PlayInvalidSelection("Not enough cash");
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
