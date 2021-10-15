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
    [SerializeField] private RectTransform RewardPanel;
    [SerializeField] private GameObject interactablePanel;
    [SerializeField] private List<OfferBox> Boxes;
    [SerializeField] private List<TurretSlotGUI> slotsGUI;
    [SerializeField] private float speed;
    [SerializeField] private float rightInitialPositon;
    [SerializeField] private float leftInitialPosition;
    [SerializeField] private float meetUpPoint;
    [SerializeField] private Camera mainCamera;

    public void InitiateGUI()
    {
        StopAllCoroutines();
        StartCoroutine(MoveRightPanel(Vector2.right * meetUpPoint));
        StartCoroutine(MoveLeftPanel(Vector2.right * meetUpPoint));
        StartCoroutine(ExpandRewardPanel(610));
        StartCoroutine(AdjustCamera(10));
    }

    public void TerminateGUI()
    {
        StopAllCoroutines();
        StartCoroutine(MoveRightPanel(Vector2.right * rightInitialPositon));
        StartCoroutine(MoveLeftPanel(Vector2.right * leftInitialPosition));
        StartCoroutine(ExpandRewardPanel(0));
        StartCoroutine(AdjustCamera(25));
    }

    private void InitiateInteractablePanel()
    {
        interactablePanel.SetActive(true);
        foreach (TurretSlotGUI slot in slotsGUI)
        {
            slot.ActivateTracking();
        }
    }

    private void TerminateInteractablePanel()
    {
        interactablePanel.SetActive(false);
        foreach (TurretSlotGUI slot in slotsGUI)
        {
            slot.DeactivateTracking();
        }
    }

    private IEnumerator MoveRightPanel(Vector2 targetPos)
    {
        float step = 0;

        while(step <= 1)
        {
            Vector2 newPos = Vector2.Lerp(rightPanel.anchoredPosition, targetPos, step);
            rightPanel.anchoredPosition = newPos;
            step += .001f;
            yield return new WaitForSecondsRealtime(.01f);
        }

        StopCoroutine("MoveRightPanel");
    }


    private IEnumerator MoveLeftPanel(Vector2 targetPos)
    {
        float step = 0;

        while(step <= 1)
        {
            Vector2 newPos = Vector2.Lerp(leftPanel.anchoredPosition, targetPos, step);
            leftPanel.anchoredPosition = newPos;
            step += .001f;
            yield return new WaitForSecondsRealtime(.01f);
        }

    }

    private IEnumerator ExpandRewardPanel(int targetSize)
    {
        float step = 0;

        while(step <= 1)
        {
            float _size = Mathf.Lerp(RewardPanel.sizeDelta.x, targetSize, step);
            RewardPanel.sizeDelta = new Vector2(_size, RewardPanel.sizeDelta.y);
            step += 0.001f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        StopCoroutine("ExpandRewardPanel");

    }

    

    private IEnumerator AdjustCamera(int targetSize)
    {
        float sign = Mathf.Sign(mainCamera.orthographicSize - targetSize);

        while((mainCamera.orthographicSize - targetSize) * sign > 0)
        {
            mainCamera.orthographicSize -= (15f/120f) * sign;
            mainCamera.transform.position += Vector3.left * (12f/120f) * sign;
            yield return new WaitForSecondsRealtime(.01f);
        }

        if(sign < 0)
        {
            TerminateInteractablePanel();
        } else 
        {
            InitiateInteractablePanel();
        }


        StopCoroutine("AdjustCamera");
    }

    public List<OfferBox> GetBoxes()
    {
        return Boxes;
    }
}
