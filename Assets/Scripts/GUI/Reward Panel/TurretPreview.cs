using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StringHandler;
using CraftyUtilities;
using System;

public class TurretPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameAndLevel;
    [SerializeField] private PreviewStatBox health;
    [SerializeField] private PreviewStatBox rest;
    [SerializeField] private PreviewStatBox damage;
    [SerializeField] private PreviewStatBox secondaryStat;
    [SerializeField] private PreviewStatBox specialStat;
    private RectTransform rect;

    
    void Start()
    {
        rect = GetComponent<RectTransform>();
        RewardManager.Main.OnRewardSelection += Deactivate;
    }

    private void Deactivate(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }

    public void ReceiveInformation(TurretManager turret)
    {
        //var shooter = turret.actionController.GetShooters()[0];

        //nameAndLevel.text = turret.baseEffect.name + " " + turret.actionController.name + "\nLevel " + turret.Level;

        //health.statName.text = StatColorHandler.HealthPaint(Stat.Health.ToSplittedString()) + ":";
        //health.statValue.text = turret.integrityManager.GetMaxIntegrity().ToString();

        //rest.statName.text = StatColorHandler.RestPaint(Stat.Rest.ToSplittedString()) + ":";
        //rest.statValue.text = shooter.StatSet[Stat.Rest].ToString();

        //damage.statName.text = StatColorHandler.DamagePaint(Stat.Damage.ToSplittedString()) + ":";
        //damage.statValue.text = shooter.StatSet[Stat.Damage].ToString();

        //secondaryStat.statName.text = StatColorHandler.StatPaint(shooter.secondaryStat.ToSplittedString()) + ":";
        //secondaryStat.statValue.text = shooter.StatSet[shooter.secondaryStat].ToString();
        
        //specialStat.statName.text = StatColorHandler.StatPaint(shooter.specializedStat.ToSplittedString()) + ":";
        //specialStat.statValue.text = shooter.StatSet[shooter.specializedStat].ToString();
    }
    
    void Update()
    {
        rect.FollowMouse();
    }
}

[System.Serializable]
public struct PreviewStatBox
{
    public TextMeshProUGUI statName;
    public TextMeshProUGUI statValue;
}
