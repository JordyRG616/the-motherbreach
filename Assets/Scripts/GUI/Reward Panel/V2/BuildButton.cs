using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuildButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public enum ButtonMode {BUILD, DONE, UPGRADE, EVOLVE};

    [SerializeField] private Sprite clickedSprite;
    private Sprite ogSprite;
    private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;
    private RewardManager rewardManager;
    private BuildBox buildBox;
    private UpgradeButton upgradeButton;
    [HideInInspector] public ButtonMode mode = ButtonMode.BUILD;
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;
    private float counter;
    [SerializeField] private float pressTime;
    private bool pressed;
    [SerializeField] private RectMask2D mask;

    void Start()
    {
        textMesh = image.GetComponentInChildren<TextMeshProUGUI>();
        ogSprite = image.sprite;
        buildBox = FindObjectOfType<BuildBox>();
        rewardManager = RewardManager.Main;
        upgradeButton = FindObjectOfType<UpgradeButton>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        pressed = true;
        //image.sprite = clickedSprite;

    }

    private void Activate()
    {
        // textMesh.color = Color.white;
        if (mode == ButtonMode.BUILD) Build();
        if (mode == ButtonMode.DONE) Done();
        if (mode == ButtonMode.UPGRADE) Upgrade();
    }

    private void Upgrade()
    {
        if(rewardManager.TotalCash >= buildBox.GetUpgradeCost())
        {
            rewardManager.SpendCash((int)buildBox.GetUpgradeCost());
            AudioManager.Main.RequestGUIFX(clickSFX);
            buildBox.UpgradeTurret();
        }
        else AudioManager.Main.PlayInvalidSelection("Not enough cash");
    }

    private void Done()
    {
        AudioManager.Main.RequestGUIFX(clickSFX);
        if(buildBox.baseToReplace)
        {
            FindObjectOfType<SellButton>(true).Replace();
        }
        upgradeButton.Disable();
        // FindObjectOfType<SellButton>().Disable();
    }

    private void Build()
    {
        if (buildBox.Selections().Weapon != null && buildBox.Selections().Base != null && rewardManager.ActiveSelection == null)
        {
            if(rewardManager.TotalCash >= buildBox.TotalCost)
            {
                AudioManager.Main.RequestGUIFX(clickSFX);
                rewardManager.SetSelection(buildBox.Selections().Weapon, buildBox.Selections().Base);
                rewardManager.SpendCash((int)buildBox.TotalCost);
            }
            else AudioManager.Main.PlayInvalidSelection("Not enough cash");
        }
        else
        {
            if(buildBox.Selections().Weapon == null && buildBox.Selections().Base == null)
            {
                AudioManager.Main.PlayInvalidSelection("Select a weapon and a foundation");
                return;
            }
            if(buildBox.Selections().Weapon == null) 
            {
                AudioManager.Main.PlayInvalidSelection("Select a weapon first");
                return;
            }
            if(buildBox.Selections().Base == null)
            {
                AudioManager.Main.PlayInvalidSelection("Select a foundation first");
                return;
            } 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //image.sprite = ogSprite;
        pressed = false;
        counter = 0;
    }

    void Update()
    {
        if (pressed)
        {
            counter += Time.deltaTime;
            if (counter >= pressTime)
            {
                Activate();
                pressed = false;
                counter = 0;
            }
        }

        mask.padding = new Vector4(0, 0, 0, 40 * (1 - (counter / pressTime)));

        if(buildBox.baseToReplace)
        {
            textMesh.text = "replace";
            return;
        }

        textMesh.text = mode.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }
}
