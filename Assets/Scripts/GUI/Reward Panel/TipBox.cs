using System.Collections;
using CraftyUtilities;
using UnityEngine;

public class TipBox : MonoBehaviour
{
    private RectTransform rect;
    private Camera cam;
    private Vector2 leftPivot = new Vector2(0, 1);
    private Vector2 rightPivot = new Vector2(1, 1);

    void OnEnable()
    {
        SetPivot();
    }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    void Update()
    {
        rect.FollowMouse();
    }

    private void SetPivot()
    {
        var pos = cam.ScreenToViewportPoint(Input.mousePosition);
        if(pos.x <= 0.5f) rect.pivot = leftPivot;
        else rect.pivot = rightPivot;
    }
}
