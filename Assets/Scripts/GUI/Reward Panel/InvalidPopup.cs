using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CraftyUtilities;

public class InvalidPopup : MonoBehaviour
{
    #region Singleton
    private static InvalidPopup _instance;
    public static InvalidPopup Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<InvalidPopup>(true);
            }

            return _instance;
        }
    }
    #endregion

    private RectTransform rect;
    private TextMeshProUGUI textMesh;
    private PopFadeTextAnimation anim;
    private bool onPop;
    private WaitForSecondsRealtime popTime = new WaitForSecondsRealtime(.5f);


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        textMesh = GetComponent<TextMeshProUGUI>();
        anim = GetComponent<PopFadeTextAnimation>();
    }

    public void PopInvalidText(string text)
    {
        gameObject.SetActive(true);
        rect.localScale = new Vector3(0, 0, 1);
        rect.FollowMouse();
        var color = textMesh.color;
        color.a = 1;
        textMesh.color = color;
        textMesh.text = text;

        StartCoroutine(Pop());
    }

    private IEnumerator Pop()
    {
        yield return StartCoroutine(anim.Forward());

        yield return popTime;

        yield return StartCoroutine(anim.Reverse());
        gameObject.SetActive(false);
    }
}
