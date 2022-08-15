using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UnlockButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UnityEvent OnClick;
    [SerializeField] private RectMask2D mask;
    [SerializeField] private float pressTime;
    private float counter;
    private bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        var panel = FindObjectOfType<SelectedShipPanel>();
        if (!panel.HasEnoughCore())
        {
            AudioManager.Main.PlayInvalidSelection("Not enough cores");
            return;
        }
        pressed = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ShaderAnimation>().Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        counter = 0;
    }

    private void Update()
    {
        if (pressed)
        {
            counter += Time.deltaTime;

            if (counter >= pressTime)
            {
                OnClick?.Invoke();
                pressed = false;
                counter = 0;
            }
        }

        mask.padding = new Vector4(0, 0, 100 * (1 - (counter / pressTime)));
    }
}
