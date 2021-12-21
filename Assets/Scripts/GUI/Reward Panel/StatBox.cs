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

    private void Update()
    {
        if(statInfoBox.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition + new Vector3(2, -2) - new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0);
            statInfoBox.anchoredPosition = mousePos;
        }
    }

}
