using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEffect : BaseEffectTemplate
{
    [SerializeField] private GameObject subEmitter;

    public override void ApplyEffect()
    {
        for(int i = 0; i < associatedController.GetShooters().Count; i++)
        {
            var shooter = associatedController.GetShooters()[i].GetShooterSystem();
            var container = Instantiate(subEmitter, Vector3.zero, subEmitter.transform.rotation, shooter.transform);
            container.GetComponent<ActionEffect>().Initiate();
            var sub = shooter.subEmitters;
            sub.AddSubEmitter(container.GetComponent<ParticleSystem>(), ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritColor);
            sub.enabled = true;
        }
    }

    public override string DescriptionText()
    {
        return "the shots of this turret explodes on contact";
    }
}
