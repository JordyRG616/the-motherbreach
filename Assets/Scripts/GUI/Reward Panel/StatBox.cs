using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform statInfoBox;
    private string statName;

    void Start()
    {
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
    }

    public void SetStatName(Stat stat)
    {
        statName = stat.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().ReceiveInfo(statName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statInfoBox.gameObject.SetActive(false);
    }
}
