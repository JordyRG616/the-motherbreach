using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProgramSelectionBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private ProgramBox programBox;
    private Image icon;
    private Program cachedProgram;
    private string description;
    private StatInfoBox statInfoBox;

    private void Start()
    {
        statInfoBox = FindObjectOfType<StatInfoBox>(true);
        icon = GetComponent<Image>();
    }

    public void ReceiveProgram(Program program)
    {
        icon.sprite = program.sprite;
        icon.enabled = true;
        cachedProgram = program;
        description = program.Description();
    }

    public void Clear()
    {
        cachedProgram = null;
        icon.enabled = false;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (description == "") return;
        if (!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statInfoBox.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        programBox.SelectProgram(cachedProgram);
    }
}
