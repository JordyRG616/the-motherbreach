using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuildButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public enum ButtonMode {BUILD, DONE, UPGRADE, EVOLVE};

    [SerializeField] private Image buildButton;
    [SerializeField] private GameObject blinkingText;
    [SerializeField] private GameObject upgradingText;
    [SerializeField] private GameObject selectText;
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
        buildBox = FindObjectOfType<BuildBox>();
        rewardManager = RewardManager.Main;
        upgradeButton = FindObjectOfType<UpgradeButton>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        pressed = true;

    }

    private void Activate()
    {
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
    }

    private void Build()
    {
        if (buildBox.Selections().Weapon != null && buildBox.Selections().Base != null && rewardManager.ActiveSelection == null)
        {
            if(rewardManager.TotalCash >= buildBox.TotalCost)
            {
                AudioManager.Main.RequestGUIFX(clickSFX);
                rewardManager.SetSelection(buildBox.Selections().Weapon, buildBox.Selections().Base);
            }
            else AudioManager.Main.PlayInvalidSelection("Not enough cash");
        }
    }

    private void SetGraphics()
    {
        if(rewardManager.ActiveSelection != null)
        {
            buildButton.enabled = false;
            textMesh.gameObject.SetActive(false);
            selectText.SetActive(true);
            blinkingText.SetActive(false);
            upgradingText.SetActive(false);
            return;
        }

        if(mode == ButtonMode.BUILD)
        {
            selectText.SetActive(false);
            upgradingText.SetActive(false);
            if (buildBox.selectedWeapon && buildBox.selectedBase)
            {
                buildButton.enabled = true;
                textMesh.gameObject.SetActive(true);
                blinkingText.SetActive(false);
            } else
            {
                buildButton.enabled = false;
                textMesh.gameObject.SetActive(false);
                blinkingText.SetActive(true);
            }
        }

        if (mode == ButtonMode.UPGRADE)
        {
            selectText.SetActive(false);
            blinkingText.SetActive(false);
            if (buildBox.selectedBaseBox || buildBox.selectedWeaponBox)
            {
                buildButton.enabled = true;
                textMesh.gameObject.SetActive(true);
                upgradingText.SetActive(false);
            }
            else
            {
                buildButton.enabled = false;
                textMesh.gameObject.SetActive(false);
                upgradingText.SetActive(true);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        counter = 0;
    }

    void Update()
    {
        SetGraphics();

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

        textMesh.text = mode.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.GetComponent<ShaderAnimation>().Play();
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }
}
