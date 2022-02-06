using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialBox : MonoBehaviour
{
    [SerializeField] private List<TutorialPointer> arrows;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButton;
    private RectTransform rect;
    private Vector2 boxPosition;
    private bool showOptions;
    private int count;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void ReceiveTutorialInfo(Vector2 boxPosition, string text, Direction arrowPosition, int lines, bool showOptions = false)
    {
        gameObject.SetActive(false);
        tutorialText.text = text;
        ActivateArrow(arrowPosition);
        this.boxPosition = boxPosition;
        this.showOptions = showOptions;
        count = lines;
        Invoke("SetSize", .1f);
    }

    internal void Terminate()
    {
        gameObject.SetActive(false);
    }

    private void ShowOptions()
    {
        yesButton.SetActive(true);
        noButton.SetActive(true);
    }

    private void SetSize()
    {
        gameObject.SetActive(true);
        var vector = new Vector2 (rect.sizeDelta.x, (count + 1) * 20);
        rect.sizeDelta = vector;
        rect.anchoredPosition = boxPosition;
        if(showOptions) ShowOptions();
    }

    private void ActivateArrow(Direction position)
    {
        foreach(TutorialPointer arrow in arrows)
        {
            if(arrow.position == position) arrow.arrow.SetActive(true);
            else arrow.arrow.SetActive(false);
        }
    }
}

[System.Serializable]
public struct TutorialPointer
{
    public GameObject arrow;
    public Direction position;
}