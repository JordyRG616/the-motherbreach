using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipAbilityGUI : MonoBehaviour
{
    [SerializeField] private RectTransform fill;
    [SerializeField] private RectTransform completedFill;
    [SerializeField] private Image icon;
    private Material _material;

    void Start()
    {
        fill.localScale = new Vector3(0, 1, 1);
        completedFill.localScale = new Vector3(1, 0, 1);

        _material = new Material(icon.material);
        icon.material = _material;
    }

    public void ReceiveIcon(Sprite abilityIcon)
    {
        icon.sprite = abilityIcon;
    }

    public void SetFillPercentage(float percentage)
    {
        if(percentage < 0) percentage = 0;
        if(percentage > 1) percentage = 1;

        fill.localScale = new Vector3(percentage, 1, 1);

        if(percentage == 1) StartCoroutine(TriggerCompletedFill());
        if(percentage == 0) 
        {
            _material.SetInt("_Blinking", 0);
            completedFill.localScale = new Vector3(1, 0, 1);
        }
    }

    private IEnumerator TriggerCompletedFill()
    {
        float step = 0;

        while(step < 1)
        {
            completedFill.localScale = new Vector3(1, step, 1);

            step += 0.1f;

            yield return new WaitForSeconds(0.01f);
        }

        completedFill.localScale = new Vector3(1, 1, 1);
        _material.SetInt("_Blinking", 1);
    }
}
