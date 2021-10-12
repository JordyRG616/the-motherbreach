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
        StartCoroutine(MoveRightPanel(Vector2.right * 150));
        StartCoroutine(MoveLeftPanel(Vector2.right * 150));
        StartCoroutine(AdjustCamera(10));
    }

    public void TerminateGUI()
    {
        StopAllCoroutines();
        StartCoroutine(MoveRightPanel(Vector2.left * 650));
        StartCoroutine(MoveLeftPanel(Vector2.right * 650));
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
        while ((rightPanel.anchoredPosition - targetPos).magnitude > 0)
        {
            rightPanel.anchoredPosition += (targetPos - rightPanel.anchoredPosition).normalized * speed;
        
            yield return new WaitForSecondsRealtime(.01f);
        }

        StopCoroutine("MoveRightPanel");
    }


    private IEnumerator MoveLeftPanel(Vector2 targetPos)
    {
        while ((leftPanel.anchoredPosition - targetPos).magnitude > 0)
        {
            leftPanel.anchoredPosition += (targetPos - leftPanel.anchoredPosition).normalized * speed * 5f/8f;
        
            yield return new WaitForSecondsRealtime(.01f);
        }

        StopCoroutine("MoveLeftPanel");
    }

    

    private IEnumerator AdjustCamera(int targetSize)
    {
        float sign = Mathf.Sign(mainCamera.orthographicSize - targetSize);

        mainCamera.GetComponent<CameraFollowComponent>().enabled = !mainCamera.GetComponent<CameraFollowComponent>().enabled;

        while((mainCamera.orthographicSize - targetSize) * sign > 0)
        {
            mainCamera.orthographicSize -= (15f/80f) * sign;
            mainCamera.transform.position += Vector3.left * (4f/80f) * sign;
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
