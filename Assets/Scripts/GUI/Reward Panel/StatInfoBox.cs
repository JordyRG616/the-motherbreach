using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatInfoBox : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    private DescriptionDictionary dictionary;
    private string statName;

    public void ReceiveInfo(string statName)
    {
        dictionary = DescriptionDictionary.Main;

        this.statName = statName;

        SetTexts();
    }

    private void SetTexts()
    {
        title.text = statName;
        description.text = dictionary.GetDescription(statName);
    }
}
