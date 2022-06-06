using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SeekerParticleComponent : MonoBehaviour
{
    [SerializeField] private AnimationCurve speedCurve;
    public float speedMagnitude;
    public int count;

    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] particles;
    private Transform target;
    private ActionEffect shooter;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        var main = _particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];

        shooter = GetComponentInParent<ActionEffect>();
    }

    void LateUpdate()
    {
        if(!_particleSystem.isPlaying || GameManager.Main.onPause) return;

        if(shooter.GetTarget() == null) return;

        target = shooter.GetTarget().transform;
        count = _particleSystem.GetParticles(particles);

        for(int i = 0; i < count; i++)
        {
            var direction = target.position - particles[i].position;
            var velocityDirection = particles[i].velocity.normalized;
            direction.z = 0;
            particles[i].velocity = speedMagnitude * (direction.normalized * speedCurve.Evaluate(1 - NormalizedLifetime(particles[i])) + velocityDirection);

            // var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // particles[i].rotation = angle;
        }

        _particleSystem.SetParticles(particles);
        // particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
    }

    private float NormalizedLifetime(ParticleSystem.Particle particle)
    {
        return particle.remainingLifetime / particle.startLifetime;
    }

    private void Seek(ParticleSystem.Particle particle)
    {
        Vector3 direction = target.position - particle.position;
        particle.velocity = Vector3.zero;
    }


}
