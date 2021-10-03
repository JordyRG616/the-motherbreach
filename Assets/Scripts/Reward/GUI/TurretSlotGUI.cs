using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSlotGUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TrackingDevice tracking;
    [SerializeField] private TurretSlot associatedSlot;
    private RewardManager manager;

    public void ActivateTracking()
    {
        if(manager == null)
        {
            manager = RewardManager.Main;

            tracking = GetComponent<TrackingDevice>();
        }
        tracking.StartTracking(associatedSlot.transform);
    }

    public void DeactivateTracking()
    {
        tracking.StopTracking();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("w");
        if(!associatedSlot.IsOcuppied())
        {
            associatedSlot.BuildTurret(manager.ActiveSelection);
            manager.BuildSelection();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(manager.ActiveSelection != null)
        {
            manager.ActiveSelection.transform.rotation = associatedSlot.transform.rotation;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(manager.ActiveSelection != null)
        {
            manager.ActiveSelection.transform.rotation = Quaternion.identity;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}