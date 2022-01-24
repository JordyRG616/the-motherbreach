using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurretSlotGUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TrackingDevice tracking;
    [SerializeField] private TurretSlot associatedSlot;
    [SerializeField] private Color color;
    private RectTransform sellButton;
    private RectTransform upgradeButton;
    private RewardManager manager;
    private RectTransform selfRect;
    private Vector3 offset;
    private InputManager inputManager;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;
    private ParticleSystem selectedVFX;

    void OnEnable()
    {
        if(manager == null)
        {
            manager = RewardManager.Main;
            inputManager = InputManager.Main;
            inputManager.OnSelectionClear += StopVFX;

            selectedVFX = associatedSlot.GetComponentInChildren<ParticleSystem>(true);

            tracking = GetComponent<TrackingDevice>();

            sellButton = FindObjectOfType<SellButton>(true).GetComponent<RectTransform>();
            upgradeButton = FindObjectOfType<UpgradeButton>(true).GetComponent<RectTransform>();

            selfRect = GetComponent<RectTransform>();

            offset = new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2);
        }

        if(!associatedSlot.IsOcuppied()) GetComponent<Image>().color = color;
        
        // tracking.StartTracking(associatedSlot.transform);
    }

    public void StopVFX(object sender, EventArgs e)
    {
        selectedVFX.Stop();
    }

    public void DeactivateSprite()
    {
        var _color = color;
        _color.a = 0;
        GetComponent<Image>().color = _color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
            AudioManager.Main.RequestGUIFX(clickSFX);
            selectedVFX.Play();
            ShowOptions();
            return;
        }
        AudioManager.Main.PlayInvalidSelection();
    }

    private void ShowOptions()
    {
        sellButton.gameObject.SetActive(true);

        int refund = (int)associatedSlot.occupyingTurret.GetComponent<TurretManager>().Level;
        if(refund < 1) refund = 1;

        sellButton.GetComponent<SellButton>().SetButton(refund, associatedSlot);

        upgradeButton.gameObject.SetActive(true);

        upgradeButton.GetComponent<UpgradeButton>().SetButton(associatedSlot);

        sellButton.anchoredPosition = Camera.main.WorldToScreenPoint(associatedSlot.transform.position) + new Vector3(0, 110) - offset;
        upgradeButton.anchoredPosition = Camera.main.WorldToScreenPoint(associatedSlot.transform.position) + new Vector3(0, 50) - offset;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if(manager.ActiveSelection != null)
        {
            manager.ActiveSelection.transform.rotation = Quaternion.identity;
            foreach(TurretVFXManager vfx in manager.ActiveSelection.GetComponentsInChildren<TurretVFXManager>()) vfx.SetSelectedColor(false);

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    void FixedUpdate()
    {
        GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(associatedSlot.transform.position);
    }
}
