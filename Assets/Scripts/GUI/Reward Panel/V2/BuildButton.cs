using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class BuildButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public enum ButtonMode {BUILD, DONE};


    [SerializeField] private Sprite clickedSprite;
    private Sprite ogSprite;
    private TextMeshProUGUI textMesh;
    private Image image;
    private RewardManager rewardManager;
    private BuildBox buildBox;
    [HideInInspector] public ButtonMode mode = ButtonMode.BUILD;
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;



    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
        ogSprite = image.sprite;
        buildBox = FindObjectOfType<BuildBox>();
        rewardManager = RewardManager.Main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = clickedSprite;
        textMesh.color = Color.white;
        if(mode == ButtonMode.BUILD) Build();
        if (mode == ButtonMode.DONE) Done();
    }

    private void Done()
    {
        AudioManager.Main.RequestGUIFX(clickSFX);
        FindObjectOfType<UpgradeButton>().Disable();
    }

    private void Build()
    {
        if (buildBox.Selections().Weapon != null && buildBox.Selections().Base != null)
        {
            AudioManager.Main.RequestGUIFX(clickSFX);
            rewardManager.SetSelection(buildBox.Selections().Weapon, buildBox.Selections().Base);
        }
        else AudioManager.Main.PlayInvalidSelection();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = ogSprite;
        textMesh.color = Color.black;
    }

    void Update()
    {
        textMesh.text = mode.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }
}
