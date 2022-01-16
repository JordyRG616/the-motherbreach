using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject cachedWeapon;
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

    public void GenerateNewWeapon(RewardLevel level)
    {
        cachedWeapon = TurretConstructor.Main.GetTop(level);
        var sprite = cachedWeapon.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
        image.color = Color.white;
        GetComponentsInChildren<RectTransform>()[1].localScale = new Vector2(1, 0);
        GetComponentInChildren<ExpandAnimation>().Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(buildBox.CheckCompability(cachedWeapon.GetComponent<ActionController>()) && !buildBox.CheckWeaponBox(this)) 
        {
            AudioManager.Main.RequestGUIFX(clickSFX);
            buildBox.ReceiveWeapon(cachedWeapon, this);
            selected = true;
            return;
        }
        if(buildBox.CheckWeaponBox(this))
        {
            AudioManager.Main.RequestGUIFX(returnSFX);
            buildBox.ClearWeapon(out cachedWeapon);
            image.color = Color.white;
            selected = false;
            return;
        } 
        if(!buildBox.CheckCompability(cachedWeapon.GetComponent<ActionController>())) AudioManager.Main.PlayInvalidSelection();
    }

    public void Detach()
    {
        cachedWeapon = null;
        image.color = Color.clear;
    }

    public void Clear()
    {
        Destroy(cachedWeapon);
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
