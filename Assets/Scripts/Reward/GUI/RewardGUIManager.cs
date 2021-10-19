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
    [SerializeField] private Image title;
    [SerializeField] [ColorUsage(true, true)] private Color textColor;
    [SerializeField] private List<RectTransform> boxRects;
    [SerializeField] private List<RectTransform> textBoxes;
    [SerializeField] private GameObject interactablePanel;
    [SerializeField] private List<OfferBox> Boxes;
    [SerializeField] private List<TurretSlotGUI> slotsGUI;
    [SerializeField] private float speed;
    [SerializeField] private float rightInitialPositon;
    [SerializeField] private float leftInitialPosition;
    [SerializeField] private float meetUpPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float sign = -1;

    public void InitiateGUI()
    {
        mainCamera.GetComponent<CameraFollowComponent>().enabled = false;
        
        StopAllCoroutines();

        StartCoroutine(MoveRightPanel(Vector2.right * meetUpPoint));
        StartCoroutine(MoveLeftPanel(Vector2.right * meetUpPoint));
        StartCoroutine(ExpandRewardPanel(610));
        StartCoroutine(AdjustCamera(10));
        InitiateInteractablePanel();

        foreach(RectTransform rect in boxRects)
        {
            foreach(Image image in rect.GetComponentsInChildren<Image>())
            {
                if(image.gameObject != rect.gameObject)
                {
                    Color newColor = image.color;
                    newColor.a = 0;
                    image.color = newColor;
                }
            }
        }
    }

    public void TerminateGUI()
    {
        StopAllCoroutines();

        TerminateInteractablePanel();
        StartCoroutine(MoveRightPanel(Vector2.right * rightInitialPositon));
        StartCoroutine(MoveLeftPanel(Vector2.right * leftInitialPosition));
        StartCoroutine(ExpandRewardPanel(0));
        StartCoroutine(AdjustCamera(25));

        mainCamera.GetComponent<CameraFollowComponent>().enabled = true;

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

        while(step <= .1f)
        {
            float _size = Mathf.Lerp(RewardPanel.sizeDelta.x, targetSize, step);
            RewardPanel.sizeDelta = new Vector2(_size, RewardPanel.sizeDelta.y);
            step += 0.001f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        StartCoroutine(ExpandBoxes(120));

        StopCoroutine("ExpandRewardPanel");

    }

    private IEnumerator ExpandBoxes(float targetSize)
    {
        float step = 0;

        while(step <= 1)
        {
            foreach(RectTransform rect in boxRects)
            {
                float _size = Mathf.Lerp(rect.sizeDelta.x, targetSize, step);
                rect.sizeDelta = new Vector2(_size, rect.sizeDelta.y);
            }
            step += 0.1f;
            if(Mathf.Approximately(step, .3f))
            {
                StartCoroutine(ExpandTexts());
            }
            yield return new WaitForSecondsRealtime(.01f);
        }

        float midStep = 0;

        while(midStep <= 1)
        {
            float _alpha = Mathf.Lerp(0, 1, midStep);
             
            foreach(RectTransform rect in boxRects)
            {
                foreach(Image image in rect.GetComponentsInChildren<Image>())
                {
                    if(image.gameObject != rect.gameObject)
                    {
                        Color newColor = image.color;
                        newColor.a = _alpha;
                        image.color = newColor;
                    }
                }
            }

            midStep += 0.01f;
            yield return new WaitForSecondsRealtime(.01f);
        }
    }

    private IEnumerator ExpandTexts()
    {
        float _step = 0;

        while(_step <= 1)
        {
            foreach(RectTransform rect in textBoxes)
            {
                float _size = Mathf.Lerp(rect.localScale.y, 1, _step);
                rect.localScale = new Vector2(rect.localScale.x, _size);
            }
            _step += 0.01f;

            if(Mathf.Approximately(_step, .3f))
            {
                StartCoroutine(GlowText());
            }
            yield return new WaitForSecondsRealtime(.01f);
        }

    }

    private IEnumerator GlowText()
    {
        float step = 0;
        Color ogColor = title.color;

        while(step <= 1)
        {
            Color newColor = Color.Lerp(ogColor, textColor, step);
            title.color = newColor;
            step += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    private IEnumerator AdjustCamera(int targetSize)
    {
        sign = sign * -1f;

        float step = 0.2f;

        while(step <= 1)
        {
            float camSize = Mathf.Lerp(0, .32f, step);
            mainCamera.orthographicSize -= camSize * sign;
            
            if(sign > 0)
            {
                Vector2 target = ShipManager.Main.transform.position;
                float adjust = 1f;

                target.x -= 9.5f * adjust;


                Vector3 offset = Vector2.Lerp(mainCamera.transform.position, target, step);
                mainCamera.transform.position = offset * sign;
                mainCamera.transform.position -= new Vector3(0, 0, 10);
            }

            step += .01f;
            yield return new WaitForSecondsRealtime(.01f);
        }

        StopCoroutine("AdjustCamera");
    }

    public List<OfferBox> GetBoxes()
    {
        return Boxes;
    }
}
