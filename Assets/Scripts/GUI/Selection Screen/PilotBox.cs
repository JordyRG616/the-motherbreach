using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using StringHandler;

public class PilotBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject pilotPrefab;
    [HideInInspector] public Pilot pilot;
    [SerializeField] private Image frame;
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] private bool defaultSelection;
    [SerializeField] private GameObject unknownPortrait;
    [SerializeField] private GameObject truePortrait;
    private StatInfoBox tipbox;
    private Sprite ogSprite;
    public UnityEvent OnClickEvent;
    private bool selected;
    private bool inactive = true;

    private LaunchButton launchButton;

    void Start()
    {
        var save = DataManager.Main.metaProgressionSave;

        if(save != null && save.unlockedPilotsIndices != null)
        {
            if(save.unlockedPilotsIndices.Contains(pilot.index) && !defaultSelection)
            {
                unknownPortrait.SetActive(false);
                truePortrait.SetActive(true);
                inactive = false;
            }
        }

        pilot = pilotPrefab.GetComponent<Pilot>();
        ogSprite = frame.sprite;

        launchButton = FindObjectOfType<LaunchButton>();
        tipbox = FindObjectOfType<StatInfoBox>(true);

        if(defaultSelection) 
        {
            SetSelection();
            inactive = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selected || inactive) return;
        SetSelection();
    }

    private void SetSelection()
    {
        frame.sprite = clickedSprite;

        launchButton.ReceivePilot(pilotPrefab);
        OnClickEvent?.Invoke();

        selected = true;
    }

    public void ClearSelection()
    {
        frame.sprite = ogSprite;
        selected = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!inactive) return;
        tipbox.gameObject.SetActive(true);
        tipbox.SetText("\nThis pilot was captured. Can be rescued by finding and destroy the corresponding" + StatColorHandler.DamagePaint("jailship") + ".\n\n");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!inactive) return;
        tipbox.gameObject.SetActive(false);

    }
}
