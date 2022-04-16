using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaunchButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Animator fadePanelAnimator;
    private GameObject selectedPilot;
    private GameObject selectedShip;

    public void OnPointerClick(PointerEventData eventData)
    {
         if (selectedShip == null)
        {
            AudioManager.Main.PlayInvalidSelection();
            return;
        }
        StartCoroutine(FadeToGame());
    }

    private IEnumerator FadeToGame()
    {
        fadePanelAnimator.SetTrigger("FadeOut");

        yield return new WaitForSecondsRealtime(1f);

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
