using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestBarManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Material _material;

    void Start()
    {
        _material = new Material(spriteRenderer.material);
        spriteRenderer.material = _material;
    }

    public void SetBarPercentual(float value)
    {
        if(value == 0) value = 1;
        _material.SetFloat("_healthPercentual", value);
    }
}
