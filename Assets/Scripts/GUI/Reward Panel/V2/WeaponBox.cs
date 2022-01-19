using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;


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
    private Material _material;
    private ParticleSystem activeVFX;
    private Light2D light2D;

    [Header("Light Colors")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color selectable;
    [SerializeField] private Color notSelectable;
    

    void Start()
    {
        buildBox = FindObjectOfType<BuildBox>();
        activeVFX = GetComponentInChildren<ParticleSystem>();
        _material = new Material(GetComponent<Image>().material);
        GetComponent<Image>().material = _material;
        light2D = GetComponentInChildren<Light2D>();
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
        if(cachedWeapon == null)
        {
            AudioManager.Main.PlayInvalidSelection();
            return;
        }
        if(buildBox.CheckCompability(cachedWeapon.GetComponent<ActionController>()) && !buildBox.CheckWeaponBox(this)) 
        {
            activeVFX.Play();
            light2D.color = selectedColor;
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

    void Update()
    {
        light2D.color = notSelectable;
        if(cachedWeapon == null) return;
        if(buildBox.CheckCompability(cachedWeapon.GetComponent<ActionController>()) && cachedWeapon) light2D.color = selectable;
    }

    public void Unselect()
    {
        activeVFX.Stop();
    }

    public void Detach()
    {
        activeVFX.Stop();
        cachedWeapon = null;
        image.color = Color.clear;
    }

    public void Clear()
    {
        activeVFX.Stop();
        Destroy(cachedWeapon);
        image.color = Color.clear;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _material.SetInt("_Moving", 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(cachedWeapon == null) return;
        _material.SetInt("_Moving", 1);
        AudioManager.Main.RequestGUIFX(hoverSFX);
    }
}
