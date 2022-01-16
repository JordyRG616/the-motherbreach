using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LockButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Sprite lockedSprite;
    private Sprite unlockedSprite;
    [SerializeField] private RectTransform tipBox;
    private TextMeshProUGUI tipBoxText;
    private Image image;
    private RewardManager rewardManager;
    public bool locked {get; private set;}
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string lockSFX;
    [SerializeField] [FMODUnity.EventRef] private string unlockSFX;
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;

    void Start()
    {
        rewardManager = RewardManager.Main;
        image = GetComponent<Image>();

        unlockedSprite = image.sprite;

        tipBoxText = tipBox.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        locked = !locked;
        if(locked)
        {
            image.sprite = lockedSprite;
            AudioManager.Main.RequestGUIFX(lockSFX);
        } 
        else
        {
            image.sprite = unlockedSprite;
            AudioManager.Main.RequestGUIFX(unlockSFX);
        } 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);
        tipBoxText.text = "lock offer";
        tipBox.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tipBox.gameObject.SetActive(false);

    }

    private void Update()
    {
        if(tipBox.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition + new Vector3(2, -2) - new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0);
            tipBox.anchoredPosition = mousePos;
        }
    }
}
