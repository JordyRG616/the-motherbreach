using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitManager : MonoBehaviour, IManager
{    
    public IDamageable HealthInterface{get; private set;}
    private ParticleSystem.Particle[] particles;
    private AudioManager audioManager;

    public void DestroyManager()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(this);
    }

    void Awake()
    {
        audioManager = AudioManager.Main;

        HealthInterface = GetComponent<IDamageable>();

        particles = new ParticleSystem.Particle[1000];
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.transform.parent.parent.TryGetComponent<ActionEffect>(out ActionEffect action))
        {
            ParticleSystem shuriken = other.GetComponent<ParticleSystem>();
            int count = shuriken.GetParticles(particles);

            ParticleSystem.Particle particle = particles.OrderBy(x => (this.transform.position - x.position).magnitude).FirstOrDefault();
            other.GetComponent<ParticleSystem>().TriggerSubEmitter(0, ref particle);

            //audioManager.RequestSFX(hitSFX);

            action.totalEffect(this);   
        }
    }
}
