using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // [SerializeField] private GameObject highlight;
    [SerializeField] private TextMeshProUGUI highlightableText;
    [SerializeField] [ColorUsage(true, true)] private Color highlightColor;
    [SerializeField] private UnityEvent eventToTrigger;
    [SerializeField] [FMODUnity.EventRef] protected string clickSFX;
    [SerializeField] [FMODUnity.EventRef] protected string hoverSFX;
    private int hoverIndex;
    private int clickIndex;
    private bool clickable = true;
    private AudioManager audioManager;
    private Color ogColor;


    [Header("If expandable:")]
    [SerializeField] private Vector2 targetedSize;
    [SerializeField] private UnityEvent onExpand;
    [SerializeField] private UnityEvent onContract;
    private Vector2 ogSize;
    private RectTransform rect;

    void Start()
    {
        audioManager = AudioManager.Main;
        rect = GetComponent<RectTransform>();

        ogColor = highlightableText.color;
    }

    private IEnumerator Enable()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        clickable = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightableText.color = highlightColor;
        audioManager.RequestGUIFX(hoverSFX, out hoverIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightableText.color = ogColor;
    }

    public void Open()
    {
        StartCoroutine(Expand());
    }

    public void Close()
    {
        StartCoroutine(Contract());
    }

    private IEnumerator Expand()
    {
        highlightableText.gameObject.SetActive(false);

        float step = 0;
        ogSize = rect.sizeDelta;

        while(step < 1)
        {
            var newSize = Vector2.Lerp(ogSize, targetedSize, step);
            rect.sizeDelta = newSize;

            step += 0.1f;

            yield return new WaitForSeconds(0.01f);
        }

        rect.sizeDelta = targetedSize;

        onExpand?.Invoke();
    }

    private IEnumerator Contract()
    {
        onContract?.Invoke();

        float step = 0;

        while(step < 1)
        {
            var newSize = Vector2.Lerp(targetedSize, ogSize, step);
            rect.sizeDelta = newSize;

            step += 0.1f;

            yield return new WaitForSeconds(0.01f);
        }

        rect.sizeDelta = ogSize;

        highlightableText.gameObject.SetActive(true);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(Enable());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!clickable) return;
        audioManager.RequestGUIFX(clickSFX, out clickIndex);
        eventToTrigger.Invoke();
        clickable = false;
    }
}
