using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class GuardianEffect : ActionEffect
{
    [SerializeField] private GameObject shieldTemplate;
    private float extraHealth;

    public override void ApplyEffect(HitManager hitManager)
    {
        
    }

    public override void Shoot()
    {
        GetTarget();

        ApplyShield();
    }

    private void ApplyShield()
    {
        var shield = Instantiate(shieldTemplate, target.transform.position, Quaternion.identity, target.transform);
        shield.GetComponent<ShieldManager>().RaiseHealthByPercentage(extraHealth);
    }

    private void GetTarget()
    {
        var turrets = ShipManager.Main.turrets;
        var rdm = Random.Range(0, turrets.Count);

        target = turrets[rdm].gameObject;
    }

    public override string DescriptionText()
    {
        return "applies a " + StatColorHandler.StatPaint("shield") + " to a random turret in the ship";
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " rest time - 10%";
        else return StatColorHandler.StatPaint("next level:") + " shield health + 33%";   
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5) ReduceRest();
        else RaiseHealth();
    }

    private void RaiseHealth()
    {
        extraHealth += .33f;
    }

    private void ReduceRest()
    {
        var rest = StatSet[Stat.Rest];
        rest *= .9f;
        SetStat(Stat.Rest, rest);
    }
}
