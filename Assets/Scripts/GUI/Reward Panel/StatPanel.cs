using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatPanel : MonoBehaviour
{
    [SerializeField] private GameObject statBox;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private StatAssetList assetList;
    [SerializeField] private RectTransform mainStatsPanel;
    [SerializeField] private RectTransform specialStatsPanel;

    public void ReceiveStats(GameObject turret)
    {
        var stats = turret.GetComponent<TurretManager>().Stats;
        foreach(Stat stat in stats.Keys)
        {
            // if(stat == Stat.Cost)
            // {
            //     SetCost(stats[stat]);
            // }
            // if(stat == Stat.Health)
            // {
            //     BuildHealthBox(stats[stat]);
            // }
            // if(stat == Stat.Damage)
            // {
            //     BuildDamageBox(stats[stat]);
            // }
            // else
            // {
            //     BuildStatBox(stat, stats[stat]);
            // }

            switch(stat)
            {
                case Stat.Cost:
                    SetCost(stats[stat]);
                    break;
                case Stat.Health:
                    BuildHealthBox(stats[stat]);
                    break;
                case Stat.Damage:
                    BuildDamageBox(stats[stat]);
                    break;
                default:
                    BuildStatBox(stat, stats[stat]);
                    break;
            }

        }
    }

    private void BuildDamageBox(float value)
    {
        GameObject container = Instantiate(statBox, Vector3.zero, Quaternion.identity, mainStatsPanel);
        var icon = container.transform.Find("Icon").GetComponent<Image>();
        var valueText = container.transform.Find("Value").GetComponent<TextMeshProUGUI>();

        icon.sprite = assetList.GetIcon(Stat.Damage);
        valueText.text = value.ToString();
    }

    private void BuildHealthBox(float value)
    {
        GameObject container = Instantiate(statBox, Vector3.zero, Quaternion.identity, mainStatsPanel);
        var icon = container.transform.Find("Icon").GetComponent<Image>();
        var valueText = container.transform.Find("Value").GetComponent<TextMeshProUGUI>();

        icon.sprite = assetList.GetIcon(Stat.Health);
        valueText.text = value.ToString();
    }

    private void SetCost(float value)
    {
        costText.text = value.ToString() + "$";
    }

    private void BuildStatBox(Stat stat, float value)
    {
        GameObject container = Instantiate(statBox, Vector3.zero, Quaternion.identity, specialStatsPanel);
        var icon = container.transform.Find("Icon").GetComponent<Image>();
        var valueText = container.transform.Find("Value").GetComponent<TextMeshProUGUI>();

        icon.sprite = assetList.GetIcon(stat);
        valueText.text = value.ToString();
    }
}

