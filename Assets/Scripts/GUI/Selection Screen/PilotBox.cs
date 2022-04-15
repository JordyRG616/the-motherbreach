using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PilotBox : MonoBehaviour, IPointerClickHandler
{
    public GameObject pilotPrefab;
    [HideInInspector] public Pilot pilot;
    [SerializeField] private Image frame;
    [SerializeField] private Sprite clickedSprite;
    [SerializeField] private bool defaultSelection;
    [SerializeField] private GameObject unknownPortrait;
    [SerializeField] private GameObject truePortrait;
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
}
