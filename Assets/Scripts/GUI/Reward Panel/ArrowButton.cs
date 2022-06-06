using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private UIAnimations clickAnimation;
    [SerializeField] private UIAnimations slideAnimation;
    [SerializeField] private UIAnimations statPanelAnimation;
    private bool open = false;


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
        
        if(open == false)
        {
            clickAnimation.Play();
            slideAnimation.Play();
            statPanelAnimation.Play();
            open = true;
        } else
        {
            clickAnimation.PlayReverse();
            slideAnimation.PlayReverse();
            statPanelAnimation.PlayReverse();
            open = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

}
