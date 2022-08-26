using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LaunchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    [SerializeField] private GameObject blinkingText;
    [SerializeField] private Image image;
    [SerializeField] private GameObject launchText;
    [SerializeField] private Animator fadePanelAnimator;
    [SerializeField] private GameObject loadingIcon;
    [SerializeField] private RectMask2D mask;
    [SerializeField] private float pressTime;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string onMapSelection;
    [SerializeField] [FMODUnity.EventRef] private string OnLaunch;
    //private GameObject selectedPilot;
    private Map selectedMap = Map.None;
    private GameObject selectedShip;
    private float counter;
    private bool pressed;

    private void Start()
    {
        StartCoroutine(WaitForSelections());
    }

    private IEnumerator WaitForSelections()
    {
        while(true)
        {
            blinkingText.SetActive(true);
            launchText.SetActive(false);
            image.enabled = false;

            yield return new WaitUntil(() => selectedShip != null && selectedMap != Map.None);

            blinkingText.SetActive(false);
            image.enabled = true;
            launchText.SetActive(true);

            yield return new WaitUntil(() => selectedShip == null || selectedMap == Map.None);
        }
    }

    private IEnumerator FadeToGame()
    {
        fadePanelAnimator.SetTrigger("FadeOut");
        loadingIcon.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);

        GameManager.Main.StartGame(selectedShip, selectedMap);
    }

    public void ReceivePilot(GameObject pilotPrefab)
    {
        //selectedPilot = pilotPrefab;
    }

    public void SetSelectedMap(int index)
    {
        AudioManager.Main.RequestGUIFX(onMapSelection);
        var map = (Map)index;
        selectedMap = map;
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
