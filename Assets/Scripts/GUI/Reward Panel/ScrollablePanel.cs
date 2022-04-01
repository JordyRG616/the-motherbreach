using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollablePanel : MonoBehaviour
{
    [SerializeField] private float increment;
    [SerializeField] private int capacity;
    private RectTransform rect;
    private float maxScroll;
    private float ypos;

    public void Initiate()
    {
        rect = GetComponent<RectTransform>();
        ypos = rect.anchoredPosition.y;
        
        var count = GetComponentsInChildren<ComponentBox>().Length - capacity;
        if(count < 0) count = 0;
        maxScroll = -count * increment;
    }

    public void Scroll(float value)
    {
        var xpos = Mathf.Lerp(0, maxScroll, value);
        rect.anchoredPosition = new Vector2(xpos, ypos);
    }
}
