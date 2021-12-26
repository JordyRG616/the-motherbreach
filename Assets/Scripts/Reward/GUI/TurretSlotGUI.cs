using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSlotGUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TrackingDevice tracking;
    [SerializeField] private TurretSlot associatedSlot;
    private RectTransform sellButton;
    private RectTransform upgradeButton;
    private RewardManager manager;
    private RectTransform selfRect;

    void Start()
    {
        if(manager == null)
        {
            manager = RewardManager.Main;

            tracking = GetComponent<TrackingDevice>();

            sellButton = FindObjectOfType<SellButton>(true).GetComponent<RectTransform>();
            upgradeButton = FindObjectOfType<UpgradeButton>(true).GetComponent<RectTransform>();

            selfRect = GetComponent<RectTransform>();
        }
        
        tracking.StartTracking(associatedSlot.transform);
    }

    public void DeactivateTracking()
    {
        tracking.StopTracking();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!associatedSlot.IsOcuppied() && manager.ActiveSelection != null)
        {
            var turret = manager.ActiveSelection;
            associatedSlot.BuildTurret(turret);
            manager.BuildSelection();
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

        sellButton.anchoredPosition = selfRect.anchoredPosition + new Vector2(0, 110);
        upgradeButton.anchoredPosition = selfRect.anchoredPosition + new Vector2(0, 50);

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
}
