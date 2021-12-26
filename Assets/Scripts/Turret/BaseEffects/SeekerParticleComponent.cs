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
        GetTarget();
    }

    private void GetTarget()
    {
        var enemies = FindObjectsOfType<EnemyManager>();
        target = enemies.OrderBy
        (
            x => Vector3.Distance(x.transform.position, transform.position)
        ).FirstOrDefault().transform;
    }

    void LateUpdate()
    {
        int count = _particleSystem.GetParticles(particles);

        for(int i = 0; i < count; i++)
        {
            Vector3 direction = target.position - particles[i].position;
            particles[i].velocity += direction.normalized / direction.magnitude;
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
