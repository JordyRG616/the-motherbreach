using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaunchButton : MonoBehaviour, IPointerClickHandler
{
    private GameObject selectedPilot;
    private GameObject selectedShip;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Main.StartGame(selectedShip, selectedPilot);
    }

    public void ReceivePilot(GameObject pilotPrefab)
    {
        selectedPilot = pilotPrefab;
    }

    public void ReceiveShip(GameObject shipPrefab)
    {
        selectedShip = shipPrefab;
    }


}
