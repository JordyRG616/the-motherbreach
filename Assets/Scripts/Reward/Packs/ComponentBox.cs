using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComponentBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI componentName;
    [SerializeField] private Image componentImage;
    [SerializeField] private TextMeshProUGUI componentDescription;
    [SerializeField] private Image background;
    [SerializeField] private Color weaponColor;
    [SerializeField] private Color foundationColor;

    public void ReceiveComponent(GameObject component)
    {
        componentName.text = component.name;
        componentImage.sprite = component.GetComponent<SpriteRenderer>().sprite;

        if(component.TryGetComponent<Weapon>(out var weapon))
        {
            componentDescription.text = weapon.Description;
            background.color = weaponColor;
            componentName.color = weaponColor;
        } else
        {
            var foundation = component.GetComponent<Foundation>();
            componentDescription.text = foundation.Description();
            background.color = foundationColor;
            componentName.color = foundationColor;
        }

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        componentName.text = "";
        componentDescription.text = "";
        background.color = Color.clear;
        componentName.color = Color.white;
        componentImage.sprite = null;

        gameObject.SetActive(false);
    }
}
