using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Bases/Explosive Rounds", fileName = "Explosive Rounds")]
public class ExplosiveRounds : Trait
{
    public GameObject explosionSystem;

    public override void Initiate(Weapon weapon)
    {
        explosionSystem = Resources.Load<GameObject>("Prefabs/Shooters/Explosion v2");
        Debug.Log(explosionSystem);
        base.Initiate(weapon);
    }

    public override void ApplyEffect(Weapon weapon)
    {
        var shooters = weapon.GetComponentsInChildren<TurretActionMediator>();

        foreach(TurretActionMediator shooter in shooters)
        {
            var explosion = Instantiate(explosionSystem, shooter.transform);
            explosion.GetComponent<TurretActionMediator>().linkedWeapon = weapon;

            var sub = shooter.GetComponent<ParticleSystem>().subEmitters;
            sub.AddSubEmitter(explosion.GetComponent<ParticleSystem>(), ParticleSystemSubEmitterType.Collision, ParticleSystemSubEmitterProperties.InheritColor);
            sub.enabled = true;
        }
    }

    public override string Description()
    {
        return "This turret's bullets will explode in contact";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (ExplosiveRounds)ScriptableObject.CreateInstance(type);

        instance.explosionSystem = explosionSystem;

        return instance;
    }
}
