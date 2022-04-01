using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TweakButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
     [SerializeField] private Sprite clickedSprite;
    [SerializeField] private RectTransform tipBox;
    [SerializeField] private GameObject pointsBox;
    [SerializeField] private TextMeshProUGUI pointsTxt;

    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string openSFX;
    [SerializeField] [FMODUnity.EventRef] private string closeSFX;
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    private Sprite ogsprite;
    private TextMeshProUGUI tipBoxText;
    private Image image;
    private OfferTweaker tweaker;
    private bool open;

    void Start()
    {
        tweaker = FindObjectOfType<OfferTweaker>();
        image = GetComponent<Image>();

        ogsprite = image.sprite;

        tipBoxText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        open = !open;
        if(open)
        {
            image.sprite = clickedSprite;
            AudioManager.Main.RequestGUIFX(openSFX);
            tweaker.Open();
        } 
        else
        {
            image.sprite = ogsprite;
            AudioManager.Main.RequestGUIFX(closeSFX);
            tweaker.Close();
        } 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);
        tipBoxText.text = "adjust offer";
        tipBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tipBox.gameObject.SetActive(false);

    }

    void Update()
    {
        if(tweaker.removalPoints > 0 && !pointsBox.activeSelf) pointsBox.SetActive(true);
        if(tweaker.removalPoints == 0) pointsBox.SetActive(false);
        pointsTxt.text = tweaker.removalPoints.ToString();
    }
}
