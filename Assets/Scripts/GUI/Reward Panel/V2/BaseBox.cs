using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseBox : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private GameObject cachedBase;
    [SerializeField] private Image image;
    private BuildBox buildBox;
    private bool selected;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clickSFX;
    [SerializeField] [FMODUnity.EventRef] private string returnSFX;


    void Start()
    {
        buildBox = FindObjectOfType<BuildBox>();
    }

    public void GenerateNewBase(RewardLevel level)
    {
        cachedBase = TurretConstructor.Main.GetBase(level);
        var sprite = cachedBase.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
        image.color = Color.white;
        GetComponentsInChildren<RectTransform>()[1].localScale = new Vector2(1, 0);
        GetComponentInChildren<ExpandAnimation>().Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(buildBox.CheckCompability(cachedBase.GetComponent<BaseEffectTemplate>()) && !buildBox.CheckBaseBox(this)) 
        {
            AudioManager.Main.RequestGUIFX(clickSFX);
            buildBox.ReceiveBase(cachedBase, this);
            selected = true;
            return;
        }
        else if(buildBox.CheckBaseBox(this))
        {
            AudioManager.Main.RequestGUIFX(returnSFX);
            buildBox.ClearBase(out cachedBase);
            image.color = Color.white;
            selected = false;
            return;
        }
        AudioManager.Main.PlayInvalidSelection();
    }

    public void Detach()
    {
        cachedBase = null;
        image.color = Color.clear;
    }

    public void Clear()
    {
        Destroy(cachedBase);
        image.color = Color.clear;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }
}
