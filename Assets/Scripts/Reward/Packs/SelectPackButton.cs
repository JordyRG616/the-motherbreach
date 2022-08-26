using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectPackButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private PackBox pack;
    private float counter;
    [SerializeField] private float pressTime;
    private bool pressed;
    [SerializeField] private RectMask2D mask;
    [Header("SFX")]
    [SerializeField] [FMODUnity.EventRef] private string hoverSFX;
    [SerializeField] [FMODUnity.EventRef] private string clicksSFX;

    void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Main.RequestGUIFX(hoverSFX);
        GetComponent<ShaderAnimation>().Play();
    }


    private void Update()
    {
        if (pressed)
        {
            counter += Time.deltaTime;

            if (counter >= pressTime)
            {
                pack.SelectPack();
                AudioManager.Main.RequestGUIFX(clicksSFX);
                pressed = false;
                counter = 0;
            }
        }

        mask.padding = new Vector4(0, 0, 105 * (1 - (counter / pressTime)), 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        counter = 0;
    }
}