using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RewardGUIManager : MonoBehaviour
{
    #region Singleton
    private static RewardGUIManager _instance;
    public static RewardGUIManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<RewardGUIManager>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<RewardGUIManager>();
                }
            }
            return _instance;
        }
    }
    #endregion


    [SerializeField] private RectTransform rightPanel;
    [SerializeField] private RectTransform leftPanel;
    [SerializeField] private List<OfferBox> Boxes;
    [SerializeField] private float speed;
    [SerializeField] private float rightInitialPositon;
    [SerializeField] private float leftInitialPosition;
    [SerializeField] private float meetUpPoint;
    [SerializeField] private Camera mainCamera;

    public void StartAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(MoveRightPanel());
        StartCoroutine(MoveLeftPanel());
        StartCoroutine(AdjustCamera());
    }

    private IEnumerator MoveRightPanel()
    {
        while(rightPanel.anchoredPosition.x < 150)
        {
            if(rightPanel.position.x != 150)
            {
                rightPanel.anchoredPosition += Vector2.right * speed;
            }

            yield return new WaitForSecondsRealtime(.01f);
        }

        StopCoroutine(MoveRightPanel());
    }

    private IEnumerator MoveLeftPanel()
    {
        while(leftPanel.anchoredPosition.x > 150)
        {
            if(leftPanel.position.x != 150)
            {
                leftPanel.anchoredPosition += Vector2.left * speed * (5f/8f);
            }
            yield return new WaitForSecondsRealtime(.01f);
        }
        StopCoroutine(MoveLeftPanel());
    }

    private IEnumerator AdjustCamera()
    {
        while(mainCamera.orthographicSize > 10)
        {
            mainCamera.orthographicSize -= (15f/80f);
            mainCamera.transform.position += Vector3.left * (4f/80f);
            yield return new WaitForSecondsRealtime(.01f);
        }
        StopCoroutine(AdjustCamera());
    }

    public List<OfferBox> GetBoxes()
    {
        return Boxes;
    }
}
