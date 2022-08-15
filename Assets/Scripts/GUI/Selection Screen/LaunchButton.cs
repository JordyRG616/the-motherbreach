using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LaunchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    [SerializeField] private Animator fadePanelAnimator;
    [SerializeField] private RectMask2D mask;
    [SerializeField] private float pressTime;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string OnLaunch;
    private GameObject selectedPilot;
    private GameObject selectedShip;
    private float counter;
    private bool pressed;


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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (selectedShip == null)
        {
            AudioManager.Main.PlayInvalidSelection("Select an available ship");
            return;
        }
        pressed = true;
    }

    private void Update()
    {
        if (pressed)
        {
            counter += Time.deltaTime;
            if (counter >= pressTime)
            {
                AudioManager.Main.RequestGUIFX(OnLaunch);
                StartCoroutine(FadeToGame());
                counter = 0;
                pressed = false;
            }
        }

        mask.padding = new Vector4(0, 0, 0, 93 * (1 - (counter / pressTime)));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        counter = 0;
        pressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ShaderAnimation>().Play();
    }
}
