using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class BaseBox : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private GameObject cachedBase;
    [SerializeField] private Image image;
    private BuildBox buildBox;
    private bool selected;
    private RectTransform statInfoBox;
    private UpgradeButton upgradeButton;

    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;
    [SerializeField] [FMODUnity.EventRef] private string returnSFX;
    private Material _material;
    private ParticleSystem activeVFX;
    private Light2D light2D;

    [Header("Light Colors")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color selectable;
    [SerializeField] private Color notSelectable;
    private GameObject replacedBase;

    void Start()
    {
        buildBox = FindObjectOfType<BuildBox>();
        activeVFX = GetComponentInChildren<ParticleSystem>();
        _material = new Material(GetComponent<Image>().material);
        GetComponent<Image>().material = _material;
        light2D = GetComponentInChildren<Light2D>();
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
        upgradeButton = FindObjectOfType<UpgradeButton>();
    }

    public void GenerateNewBase()
    {
        cachedBase = TurretConstructor.Main.GetBase();
        var sprite = cachedBase.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
        image.color = Color.white;
        GetComponentsInChildren<RectTransform>()[1].localScale = new Vector2(1, 0);
        GetComponentInChildren<ExpandAnimation>().Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        
        if(cachedBase == null || RewardManager.Main.ActiveSelection)
        {
            AudioManager.Main.PlayInvalidSelection("");
            return;
        }
        if(buildBox.CheckCompability(cachedBase.GetComponent<Foundation>()) && !buildBox.CheckBaseBox(this)) 
        {
            if(!buildBox.OnUpgrade)
            {
                activeVFX.Play();
                light2D.color = selectedColor;
                AudioManager.Main.RequestGUIFX(clickSFX);
                buildBox.ReceiveBase(cachedBase, this);
                selected = true;
                return;
            }
            else if(buildBox.OnUpgrade && IsSameBase())
            {
                activeVFX.Play();
                light2D.color = selectedColor;
                AudioManager.Main.RequestGUIFX(clickSFX);
                buildBox.ReceiveBaseBox(this);
                buildBox.SetCostToBaseCost(true);
                selected = true;

                return;
            }
        }
        else if(buildBox.CheckBaseBox(this))
        {
            AudioManager.Main.RequestGUIFX(returnSFX);
            buildBox.ClearBase(out cachedBase);
            image.color = Color.white;
            selected = false;
            return;
        }
        AudioManager.Main.PlayInvalidSelection("hehehehe");
    }

    private bool IsSameBase()
    {
        if (buildBox.selectedBase == null || cachedBase == null) return false;
        return buildBox.selectedBase.GetComponent<Foundation>().Id == cachedBase.GetComponent<Foundation>().Id;
    }

    void Update()
    {
        light2D.color = notSelectable;
        if(cachedBase == null) return;
        var check = buildBox.CheckCompability(cachedBase.GetComponent<Foundation>()) && cachedBase && !buildBox.OnUpgrade;
        var secondCheck = buildBox.OnUpgrade && IsSameBase();
        if (check || secondCheck) light2D.color = selectable;
    }
    
    public void Unselect()
    {
        activeVFX.Stop();
    }


    public void Detach()
    {
        activeVFX.Stop();
        cachedBase = null;
        image.color = Color.clear;
    }

    public void Clear()
    {
        activeVFX.Stop();
        Destroy(cachedBase);
        image.color = Color.clear;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _material.SetInt("_Moving", 0);
        statInfoBox.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(cachedBase == null) return;
        _material.SetInt("_Moving", 1);
        AudioManager.Main.RequestGUIFX(hoverSFX);

        var text = "<size=150%><lowercase>" + cachedBase.name;

        if(!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(text, 200);
        }
    }
}
