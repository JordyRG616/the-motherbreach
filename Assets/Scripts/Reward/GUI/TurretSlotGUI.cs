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

    void OnEnable()
    {
        if(manager == null)
        {
            manager = RewardManager.Main;

            tracking = GetComponent<TrackingDevice>();

            sellButton = FindObjectOfType<SellButton>(true).GetComponent<RectTransform>();
            upgradeButton = FindObjectOfType<UpgradeButton>(true).GetComponent<RectTransform>();

            selfRect = GetComponent<RectTransform>();

            offset = new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2);
        }

        if(!associatedSlot.IsOcuppied()) GetComponent<Image>().color = color;
        
        // tracking.StartTracking(associatedSlot.transform);
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
        }
        else if(associatedSlot.IsOcuppied() && manager.ActiveSelection == null)
        {
            ShowOptions();
        }
    }

    private void ShowOptions()
    {
        sellButton.gameObject.SetActive(true);

        int refund = (int)associatedSlot.occupyingTurret.GetComponent<TurretManager>().Stats[Stat.Cost] / 3;
        if(refund < 1) refund = 1;

        sellButton.GetComponent<SellButton>().SetButton(refund, associatedSlot);

        upgradeButton.gameObject.SetActive(true);

        

        sellButton.anchoredPosition = Camera.main.WorldToScreenPoint(associatedSlot.transform.position) + new Vector3(0, 110) - offset;
        upgradeButton.anchoredPosition = Camera.main.WorldToScreenPoint(associatedSlot.transform.position) + new Vector3(0, 50) - offset;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(manager.ActiveSelection != null)
        {
            manager.ActiveSelection.transform.rotation = associatedSlot.transform.rotation;
            if(!associatedSlot.IsOcuppied())
            {
                manager.ActiveSelection.GetComponentInChildren<TurretVFXManager>().SetSelectedColor(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(manager.ActiveSelection != null)
        {
            manager.ActiveSelection.transform.rotation = Quaternion.identity;
            manager.ActiveSelection.GetComponentInChildren<TurretVFXManager>().SetSelectedColor(false);

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
