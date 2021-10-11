using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitManager : MonoBehaviour, IManager
{    
    public IDamageable HealthInterface{get; private set;}
    private ParticleSystem.Particle[] particles;

    public void DestroyManager()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(this);
    }

    void Awake()
    {
        HealthInterface = GetComponent<IDamageable>();

        particles = new ParticleSystem.Particle[1000];
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent<ActionEffect>(out ActionEffect action))
        {
            ParticleSystem shuriken = other.GetComponent<ParticleSystem>();
            int count = shuriken.GetParticles(particles);

            // List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>(shuriken.GetSafeCollisionEventSize());
            // int events = shuriken.GetCollisionEvents(gameObject, _collisionEvents);

            // foreach(ParticleCollisionEvent collision in _collisionEvents)
            // {
            //     collision.
            // }

            ParticleSystem.Particle particle = particles.OrderBy(x => (this.transform.position - x.position).magnitude).FirstOrDefault();
            other.GetComponent<ParticleSystem>().TriggerSubEmitter(0, ref particle);
            action.ApplyEffect(this);   
        }
    }
}
