using System.Collections;
using CraftyUtilities;
using UnityEngine;
using TMPro;

public class StatInfoBox : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private RectTransform[] rects;
    private RectTransform selfRect;
    private DescriptionDictionary dictionary;
    private string statName;
    private int largestLineSize = 0;
    private float ogWidth;
    private float _width;

    void Start()
    {
        ogWidth = rects[0].sizeDelta.x;
        _width = ogWidth;

        selfRect = GetComponent<RectTransform>();
    }

    private void SetSize()
    {
        var count = description.textInfo.lineCount;
        var height = description.textInfo.lineInfo[0].lineHeight;
        var vector = new Vector2 (_width, (count + 1) * height);
        foreach(RectTransform rect in rects)
        {
            rect.sizeDelta = vector;
        }
    }

    public void SetText(string text)
    {
        _width = ogWidth;
        description.text = string.Empty;
        description.text = text;
        // SetSize();
    }

    public void SetText(string text, float width)
    {
        _width = width;
        description.text = string.Empty;
        description.text = text;
        // SetSize();
    }

    private void SetTexts()
    {
        _width = ogWidth;
        description.text = string.Empty;
        var text = dictionary.GetDescription(statName);
        description.text = text;
        // SetSize();
    }

    void Update()
    {
        SetSize();
        selfRect.FollowMouse();
    }
}
