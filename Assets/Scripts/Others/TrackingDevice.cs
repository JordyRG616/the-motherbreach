using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingDevice : MonoBehaviour
{
    private Transform target;
    [Header("Check for UI Elements")]
    [SerializeField] private bool IsUI;

    private bool tracking;

    private IEnumerator TrackPointer()
    {
        while(tracking == true)
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

            yield return new WaitForSecondsRealtime(.01f);
        }
    }

    private IEnumerator TrackObject()
    {
        while(tracking == true)
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
            yield return new WaitForSecondsRealtime(.01f);
        }
    }

    public void StartTracking()
    {
        tracking = true;
        StartCoroutine(TrackPointer());
    }

    public void StartTracking(Transform target)
    {
        this.target = target;
        tracking = true;
        StartCoroutine(TrackObject());
    }

    public GameObject ReturnObject()
    {
        return target.gameObject;
    }

    public void StopTracking()
    {
        tracking = false;
        StopAllCoroutines();
    }
}
