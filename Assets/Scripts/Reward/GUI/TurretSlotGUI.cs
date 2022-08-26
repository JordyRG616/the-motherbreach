using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurretSlotGUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TurretSlot associatedSlot;
    [SerializeField] private Color color;
    private RectTransform sellButton;
    private bool turretOnUpgrade;
    private RewardManager manager;
    private RectTransform selfRect;
    private Vector3 offset;
    private InputManager inputManager;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;
    private ParticleSystem selectedVFX;
    private BuildBox buildBox;
    private bool initiated;
    private TurretPreview preview;

    public int index;

    void Awake()
    {
        if(!initiated)
        {
            var list = FindObjectsOfType<TurretSlot>().ToList();
            associatedSlot = list.Find(x => x.index == index);
            index ++;
            initiated = true;
            preview = FindObjectOfType<TurretPreview>(true);
        }
    }

    void OnEnable()
    {
        if(manager == null)
        {
            manager = RewardManager.Main;
            inputManager = InputManager.Main;
            inputManager.OnSelectionClear += StopVFX;
            buildBox = FindObjectOfType<BuildBox>();

            selectedVFX = associatedSlot.GetComponentInChildren<ParticleSystem>(true);

            var _sellButton = FindObjectOfType<SellButton>(true);
            sellButton = _sellButton.GetComponent<RectTransform>();
            _sellButton.OnTurretSell += ActivateSprite;
            manager.OnRewardSelection += StopVFX;

            offset = new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2);

            selfRect = GetComponent<RectTransform>();

            var position = Camera.main.WorldToViewportPoint(associatedSlot.transform.position);
            position.x *= 1280;
            position.y *= 720;
            selfRect.anchoredPosition = position;
        }

        if(!associatedSlot.IsOcuppied()) GetComponent<Image>().color = color;
        
    }

    public void StopVFX(object sender, EventArgs e)
    {
        turretOnUpgrade = false;
        selectedVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void DeactivateSprite()
    {
        var _color = color;
        _color.a = 0;
        GetComponent<Image>().color = _color;
    }

    public void ActivateSprite(object sender, EventArgs e)
    {
        if (associatedSlot.IsOcuppied()) return;
        GetComponent<Image>().color = color;
        turretOnUpgrade = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    private void SendToBuildBox()
    {   
        var _weapon = associatedSlot.occupyingTurret.GetComponentInChildren<Weapon>().gameObject;
        var _base = associatedSlot.occupyingTurret.GetComponentInChildren<Foundation>().gameObject;

        buildBox.ActivateUpgradeMode();
        buildBox.ReceiveWeapon(_weapon);
        buildBox.ReceiveBase(_base);
    }

    private void ShowOptions()
    {
        sellButton.gameObject.SetActive(true);

        int refund = (int)associatedSlot.occupyingTurret.GetComponent<TurretManager>().Level / 2;
        if(refund < 1) refund = 1;

        sellButton.GetComponent<SellButton>().SetButton(refund, associatedSlot);
        //sellButton.anchoredPosition = Camera.main.WorldToScreenPoint(associatedSlot.transform.position) + new Vector3(0, 55) - offset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        
        if(turretOnUpgrade)
        {
            selectedVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            buildBox.Clear();
            return;
        }
        if(!associatedSlot.IsOcuppied() && manager.ActiveSelection != null)
        {
            var turret = manager.ActiveSelection;
            associatedSlot.BuildTurret(turret);
            manager.BuildSelection();
            DeactivateSprite();
            return;
        }
        else if(associatedSlot.IsOcuppied() && manager.ActiveSelection == null)
        {
            inputManager.ForceSelectionClear();
            AudioManager.Main.RequestGUIFX(clickSFX);
            selectedVFX.Play();
            SendToBuildBox();
            turretOnUpgrade = true;
            ShowOptions();
            return;
        } 
        AudioManager.Main.PlayInvalidSelection("Select an empty slot");
            
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);

        if(manager.ActiveSelection != null)
        {
            manager.ActiveSelection.transform.rotation = associatedSlot.transform.rotation;
            if(!associatedSlot.IsOcuppied())
            {
                foreach(TurretVFXManager vfx in manager.ActiveSelection.GetComponentsInChildren<TurretVFXManager>()) vfx.SetSelectedColor(true);
            }
        } 
        //else if(associatedSlot.IsOcuppied())
        //{
        //    preview.gameObject.SetActive(true);
        //    preview.ReceiveInformation(associatedSlot.occupyingTurret.GetComponent<TurretManager>());
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if(manager.ActiveSelection != null)
        {
            manager.ActiveSelection.transform.rotation = Quaternion.identity;
            foreach(TurretVFXManager vfx in manager.ActiveSelection.GetComponentsInChildren<TurretVFXManager>()) vfx.SetSelectedColor(false);

        } 
        //else if(associatedSlot.IsOcuppied())
        //{
        //    preview.gameObject.SetActive(false);
        //}
    }
}
