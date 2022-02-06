using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTypeShooter : MonoBehaviour
{
    private int count;
    private EffectMediator mediator;
    private ParticleSystem ps;
    private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
    [SerializeField] private bool singleHit = true;
    private List<Collider2D> affectedColliders;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        mediator = GetComponent<EffectMediator>();
    }

    void OnParticleTrigger()
    {
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles, out var colliderData);

        for (int i = 0; i < numInside; i++)
        {
            for (int j = 0; j < colliderData.GetColliderCount(i); j++)
            {
                colliderData.GetCollider(i, j).GetComponent<HitManager>().ReceiveTriggetEffect(mediator);
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

    void Update()
    {
        GetEnemies();
    }
}
