using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardBox : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    [SerializeField] private RectTransform infoBox;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Image TopImage;
    [SerializeField] private Image baseImage;
    [SerializeField] private StatPanel statPanel;
    public bool Empty {get; private set;} = true;
    private WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.001f);
    public string weaponName {get; private set;}
    public string baseName{get; private set;}

    public event EventHandler OnOfferSelected;

    public void ReceiveTurret(GameObject turret)
    {
        SpriteRenderer[] spriteRenderers = turret.GetComponentsInChildren<SpriteRenderer>(true);
        
        TopImage.sprite = spriteRenderers[2].sprite;
        TopImage.color = spriteRenderers[2].color;
        baseImage.sprite = spriteRenderers[1].sprite;
        baseImage.color = spriteRenderers[1].color;

        weaponName = turret.GetComponentInChildren<ActionController>().gameObject.name;
        baseName = turret.GetComponentInChildren<BaseEffectTemplate>().gameObject.name;

        statPanel.ReceiveStats(turret);

        Empty = false;
    }

    public void Clear()
    {
        Empty = true;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        OnOfferSelected?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!infoBox.gameObject.activeSelf)
        {
            infoBox.gameObject.SetActive(true);
            infoBox.GetComponent<InfoBox>().ReceiveInfo(weaponName, baseName);
        }
    }

    private void Update()
    {
        if(infoBox.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition + (Vector3)offset - new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0);
            infoBox.anchoredPosition = mousePos;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.gameObject.SetActive(false);
    }
}
