using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackingDevice : MonoBehaviour
{
    private Transform target;
    [Header("Check for UI Elements")]
    [SerializeField] private bool IsUI;

    private bool trackingPointer;
    private bool trackingObject;

    private void TrackPointer()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if(EventSystem.current.currentSelectedGameObject == null) return;
        }
        if(trackingPointer == true)
        {
            Vector2 viewPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos = new Vector2(viewPos.x, viewPos.y);
            
            if(IsUI)
            {
                mousePos = Input.mousePosition;
                GetComponent<RectTransform>().anchoredPosition = mousePos;

            } else 
            {
                transform.position = mousePos;
            }
        }
    }

    private void TrackObject()
    {
        if(trackingObject == true)
        {
            Vector2 targetPos = target.position;
            if(IsUI)
            {
                targetPos = Camera.main.WorldToScreenPoint(targetPos) - new Vector3(Camera.main.pixelWidth /2, Camera.main.pixelHeight /2);
                GetComponent<RectTransform>().anchoredPosition = targetPos;

            } else 
            {
                transform.position = targetPos;
            }
        }
    }

    public void StartTracking()
    {
        trackingPointer = true;
    }

    public void StartTracking(Transform target)
    {
        this.target = target;
        trackingObject = true;
    }

    public GameObject ReturnObject()
    {
        return target.gameObject;
    }

    public void StopTracking()
    {
        trackingPointer = false;
        trackingObject = false;
    }

    public void DisableAndReset()
    {
        transform.localPosition = Vector3.zero;
        Destroy(this);
    }

    void Update()
    {
        TrackObject();
        TrackPointer();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Vector2 viewPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(viewPos.x, viewPos.y);

        // Debug.Log(mousePos);

        Gizmos.DrawWireSphere(mousePos, .5f);
        Gizmos.DrawWireSphere(transform.position, .5f);

    }
}
