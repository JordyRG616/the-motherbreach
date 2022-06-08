using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;
using System.Linq;

public class EmpoweringEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1f)] private float percentage;
    [SerializeField] private ParticleSystem enhanceVFX;
    [SerializeField] [FMODUnity.EventRef] private string enhanceSFX;
    private Dictionary<ActionController, float> targetedWeaponsOriginalDamage = new Dictionary<ActionController, float>();
    

    private EnemyDeathEvent deathEvent;
    private ActionController target;

    public override void Initiate()
    {
        base.Initiate();

        deathEvent = GetComponent<EnemyDeathEvent>();
        deathEvent.effect += ApplyEffect;

        gameManager.OnGameStateChange += ResetTurrets;
    }

    public override void ApplyEffect()
    {

        if(deathEvent.killer == associatedController.gameObject)
        {
            GetTarget();

            foreach(ActionEffect weapon in target.GetShooters())
            {
                var damage = weapon.StatSet[Stat.Damage];

                weapon.SetStat(Stat.Damage, damage * (1 + percentage));
            }

            if(gameManager.gameState == GameState.OnReward) return;

            enhanceVFX.transform.position = target.transform.position;
            enhanceVFX.Play();
            AudioManager.Main.RequestSFX(enhanceSFX);
        }
    }

    private void ResetTurrets(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward)
        {
            foreach(ActionController controller in targetedWeaponsOriginalDamage.Keys)
            {
                foreach(ActionEffect weapon in controller.GetShooters())
                {
                    weapon.SetStat(Stat.Damage, targetedWeaponsOriginalDamage[controller]);
                }
            }
        }
    }

    private void GetTarget()
    {
        TurretManager[] turrets = new TurretManager[ShipManager.Main.turrets.Count];
        ShipManager.Main.turrets.CopyTo(turrets);
        var _turrets = turrets.ToList();
        _turrets.Remove(turretManager);
        var rdm = Random.Range(0, _turrets.Count);

        //target = _turrets[rdm].actionController;

        if(!targetedWeaponsOriginalDamage.ContainsKey(target))
        {
            targetedWeaponsOriginalDamage.Add(target, target.GetShooters()[0].StatSet[Stat.Damage]);
        }
    }

    public override string DescriptionText()
    {
        return "if the enemy was killed by this turret, raise the damage of a random turret by " + StatColorHandler.StatPaint((percentage * 100).ToString()) + "% until the end of the wave";
    }
}
