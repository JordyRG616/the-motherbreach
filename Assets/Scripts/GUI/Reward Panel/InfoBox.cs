using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InfoBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponTitle;
    [SerializeField] private TextMeshProUGUI baseTitle;
    [SerializeField] private TextMeshProUGUI weaponDescription;
    [SerializeField] private TextMeshProUGUI baseDescription;
    private DescriptionDictionary dictionary;
    private string weaponName;
    private string baseName;

    public void ReceiveInfo(string weaponName, string baseName)
    {
        dictionary = DescriptionDictionary.Main;

        this.weaponName = weaponName;
        this.baseName = baseName;

        SetTexts();
    }

    private void SetTexts()
    {
        weaponTitle.text = weaponName;
        weaponDescription.text = dictionary.GetDescription(weaponName);

        baseTitle.text = baseName;
        baseDescription.text = dictionary.GetDescription(baseName);
    }
}
