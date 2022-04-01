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
    private WaitForSeconds waitTime = new WaitForSeconds(0.001f);
    public string weaponName {get; private set;}
    public string baseName{get; private set;}
    private Material topMaterial;
    private Material baseMaterial;

    public event EventHandler OnOfferSelected;

    public void ReceiveTurret(GameObject turret)
    {
        SpriteRenderer[] spriteRenderers = turret.GetComponentsInChildren<SpriteRenderer>(true);
        
        TopImage.sprite = spriteRenderers[2].sprite;
        TopImage.color = spriteRenderers[2].color;
        baseImage.sprite = spriteRenderers[1].sprite;
        baseImage.color = spriteRenderers[1].color;

        topMaterial = new Material(TopImage.material);
        baseMaterial = new Material(baseImage.material);

        TopImage.material = topMaterial;
        baseImage.material = baseMaterial;

        RewardManager.Main.StartCoroutine(DisplayTurret());

        weaponName = turret.GetComponentInChildren<ActionController>().gameObject.name;
        baseName = turret.GetComponentInChildren<BaseEffectTemplate>().gameObject.name;

        statPanel.ReceiveStats(turret);

        Empty = false;
    }

    private IEnumerator DisplayTurret()
    {
        float step = 0;

        while(step <= 2)
        {
            topMaterial.SetFloat("_Mask", step);
            baseMaterial.SetFloat("_Mask", step);

            step += 0.05f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Clear()
    {
        Empty = true;
        Color color = new Color(1, 1, 1, 0);
        TopImage.sprite = null;
        TopImage.color = color;
        baseImage.sprite = null;
        baseImage.color = color;

        topMaterial.SetFloat("_Mask", 0);
        baseMaterial.SetFloat("_Mask", 0);

        statPanel.Clear();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        OnOfferSelected?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!infoBox.gameObject.activeSelf && !Empty)
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
        if(infoBox.gameObject.activeSelf) infoBox.gameObject.SetActive(false);
    }
}
