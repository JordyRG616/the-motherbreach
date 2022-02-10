using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComponentBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image componentImage;
    private Image image;
    private string componentName;
    private RectTransform infoBox;

    void Awake()
    {
        image = GetComponent<Image>();
        infoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();

    }

    public void ConfigureBox(string name, Sprite sprite, Color color)
    {
        componentImage.sprite = sprite;
        componentName = name;
        image.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // GetComponent<ShaderAnimation>().Play();
        // AudioManager.Main.RequestGUIFX(hoverSFX);

        infoBox.gameObject.SetActive(true);
        infoBox.GetComponent<StatInfoBox>().SetText(componentName, 200);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.gameObject.SetActive(false);
    }
}
