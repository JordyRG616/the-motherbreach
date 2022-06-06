using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class GuardianEffect : ActionEffect
{
    [SerializeField] private GameObject shieldTemplate;
    private float extraHealth;
    private float initialSize = 1;

    public override Stat specializedStat => Stat.Efficiency;

    public override Stat secondaryStat => Stat.Size;


    public override void SetData()
    {
        StatSet.Add(Stat.Efficiency, extraHealth);
        StatSet.Add(Stat.Size, initialSize);
        base.SetData();
    }

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
        shield.GetComponent<ShieldManager>().RaiseHealthByPercentage(StatSet[Stat.Efficiency]);
        var size = shield.transform.localScale;
        shield.transform.localScale = size * StatSet[Stat.Size];
    }

    private void GetTarget()
    {
        var turrets = ShipManager.Main.turrets;
        turrets.Remove(GetComponentInParent<TurretManager>());

        //foreach (TurretManager turret in turrets)
        //{
        //    if (turret.GetComponentInChildren<ShieldManager>())
        //    {
        //        turrets.Remove(turret);
        //    }
        //}

        //if (turrets.Count == 0) return;

        var rdm = Random.Range(0, turrets.Count);

        target = turrets[rdm].gameObject;

        var existingShield = target.GetComponentInChildren<ShieldManager>();

        if (existingShield != null)
        {
            existingShield.DestroyShield();
        }
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
        
    }

    public override void RaiseInitialSpecializedStat(float percentage)
    {
        extraHealth += percentage;
    }

    public override void RaiseInitialSecondaryStat(float percentage)
    {
        initialSize *= 1 + percentage;
    }
}
