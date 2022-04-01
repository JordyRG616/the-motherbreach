using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class LegionaryEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;
    private float ogDamage;

    public override void Initiate()
    {
        base.Initiate();
        gameManager.OnGameStateChange += ResetTurrets;
    }

    public override void ApplyEffect()
    {
        var Count = GetLegionaryCount();
        percentage *= Count;

        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            ogDamage = shooter.StatSet[Stat.Damage];
            shooter.SetStat(Stat.Damage, ogDamage * (1 + percentage));
        }
    }

    private void ResetTurrets(object sender, GameStateEventArgs e)
    {
        if(e.newState != GameState.OnReward) return;

        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            shooter.SetStat(Stat.Damage, ogDamage);
        }
    }

    private int GetLegionaryCount()
    {
        var turrets = ShipManager.Main.turrets.FindAll(x => x.GetComponentInChildren<LegionaryEffect>());
        return turrets.Count;
    }

    public override string DescriptionText()
    {
        return "raise the damage of this turret by " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% for each legionary turret in the ship";
    }
}
