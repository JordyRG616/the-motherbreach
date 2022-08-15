using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardInfoBox : MonoBehaviour
{
    private Vector2 initialPosition;
    [SerializeField] private Vector2 finalPosition;
    private RectTransform rect;
    private bool expanding;
    private float _step;
    public float Step 
    { 
        get => _step; 
        private set
        {
            if (value > 1) value = 1;
            else if (value < 0) value = 0;
            else _step = value; 
        }
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        initialPosition = rect.anchoredPosition;
    }

    private void Update()
    {
        if (expanding) Step += Time.deltaTime * 5;
        else Step -= Time.deltaTime * 5;

        SetPosition();
    }

    private void SetPosition()
    {
        Vector2 pos = Vector2.Lerp(initialPosition, finalPosition, Step);
        rect.anchoredPosition = pos;
    }

    public void InvertDirection()
    {
        expanding = !expanding;
    }

}
