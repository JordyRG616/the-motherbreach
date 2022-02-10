using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SeekerParticleComponent : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] particles;
    private Transform target;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        var main = _particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];

        StartCoroutine(GetTarget());
    }

    private IEnumerator GetTarget()
    {
        while(target == null)
        {
            yield return new WaitUntil(() => _particleSystem.isEmitting);

            target = GetComponentInParent<ActionController>().target.transform;
        }
    }

    void LateUpdate()
    {
        if(target == null) return;
        int count = _particleSystem.GetParticles(particles);

        for(int i = 0; i < count; i++)
        {
            Vector3 direction = target.position - particles[i].position;
            particles[i].velocity += direction.normalized / particles[i].velocity.magnitude;
        }

        _particleSystem.SetParticles(particles);
        particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
    }

    private void Seek(ParticleSystem.Particle particle)
    {
        Vector3 direction = target.position - particle.position;
        particle.velocity = Vector3.zero;
    }


}
