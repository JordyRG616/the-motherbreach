using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StatBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI header, value;
    private RectTransform statInfoBox;
    public string description;

    void Start()
    {
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statInfoBox.gameObject.SetActive(false);
    }

    public void SetHeaderText(string text)
    {
        header.text = text.ToLower();
    }

    public void SetValueText(string text)
    {
        value.text = text.ToLower();
    }
}
