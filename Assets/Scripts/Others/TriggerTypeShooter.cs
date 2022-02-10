using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTypeShooter : MonoBehaviour
{
    [SerializeField] private bool isEnemy = false;
    private int count;
    private EffectMediator mediator;
    private ParticleSystem ps;
    private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
    private List<Collider2D> affectedColliders;

    private delegate void GetTargets();
    private GetTargets getTargets;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        mediator = GetComponent<EffectMediator>();

        if(!isEnemy) getTargets += GetEnemies;
        else getTargets += GetTurrets;
    }

    void OnParticleTrigger()
    {
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles, out var colliderData);

        for (int i = 0; i < numInside; i++)
        {
            for (int j = 0; j < colliderData.GetColliderCount(i); j++)
            {
                var col = colliderData.GetCollider(i, j);
                if(col == null)
                {
                    ps.trigger.RemoveCollider(j);
                }
                else col.GetComponent<HitManager>().ReceiveTriggetEffect(mediator);
            }
        }
    }

    private void GetEnemies()
    {
        var enemies = FindObjectsOfType<EnemyManager>();
        var trigger = ps.trigger;

        if(trigger.colliderCount == enemies.Length) return;
        
        for (int i = 0; i < trigger.colliderCount; i++)
        {
            trigger.RemoveCollider(i);
        }

        foreach(EnemyManager enemy in enemies)
        {
            var col = enemy.GetComponent<Collider2D>();
            trigger.AddCollider(col);
        }
    }

    private void GetTurrets()
    {
        var turrets = FindObjectsOfType<TurretManager>();
        var trigger = ps.trigger;

        if(trigger.colliderCount == turrets.Length) return;
        
        for (int i = 0; i < trigger.colliderCount; i++)
        {
            trigger.RemoveCollider(i);
        }

        foreach(TurretManager turret in turrets)
        {
            var col = turret.GetComponent<Collider2D>();
            trigger.AddCollider(col);
        }
    }

    void Update()
    {
        getTargets?.Invoke();
    }
}
