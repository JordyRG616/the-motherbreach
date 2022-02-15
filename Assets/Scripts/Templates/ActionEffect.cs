using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ActionEffect : MonoBehaviour
{
    [SerializeField] protected ParticleSystem shooterParticle;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected WeaponClass weaponClass;
    [SerializeField] protected Keyword keyword;
    protected GameObject target;
    [SerializeField] protected float initialDamage;
    [SerializeField] protected float initialRest;
    public Dictionary<Stat, float> StatSet {get; protected set;} = new Dictionary<Stat, float>();
    protected GameManager gameManager;
    [SerializeField] [FMODUnity.EventRef] protected string onShootSFX;
    protected FMOD.Studio.EventInstance sfxInstance;

    public delegate void Effect(HitManager hitManager);

    public Effect totalEffect;
    private bool shooting;
    [SerializeField] protected bool singleSFX;
    private int cachedCount = 0;
    private int count = 0;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[50];
    private List<ParticleSystem.Particle> particlesList = new List<ParticleSystem.Particle>();


    public virtual void Initiate()
    {
        shooterParticle.Stop(true);

        SetData();

        totalEffect += ApplyEffect;

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += ClearShots;

    }

    public abstract string DescriptionText();
    public virtual string DescriptionText(out Keyword keyword)
    {
        keyword = this.keyword;
        return DescriptionText();
    }

    public virtual string upgradeText(int nextLevel)
    {
        return "";
    }

    protected virtual void ClearShots(object sender, GameStateEventArgs e)
    {
        shooterParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        // shooter.Clear();
    }

    public abstract void LevelUp(int toLevel);

    public WeaponClass GetClass()
    {
        return weaponClass;
    }

    public virtual void SetData()
    {
        // StatSet.Clear();
        StatSet.Add(Stat.Damage, initialDamage);
        StatSet.Add(Stat.Rest, initialRest);

        var col = shooterParticle.collision;
        col.collidesWith = targetLayer;
    }

    public virtual void SetStat(Stat statName, float value)
    {
        value = (float)Math.Round(value, 1);
        if(StatSet.ContainsKey(statName))
        {
            StatSet[statName] = value;
        } 
    }

    public virtual void ReceiveTarget(GameObject parentTarget)
    {
        target = parentTarget;
    }

    public virtual void Shoot()
    {
        if(target != null && !shooterParticle.isEmitting)
        {
            if(singleSFX) AudioManager.Main.RequestSFX(onShootSFX, out sfxInstance);
            shooting = true;
            shooterParticle.Play(true);
        }
    }

    protected virtual void ManageSFX()
    {
        if(!singleSFX && shooterParticle.isEmitting)
        {
            count = shooterParticle.particleCount; //shooterParticle.GetParticles(particles);

            if(cachedCount < count)
            { 
                for(int i = 0; i <= count - cachedCount; i++)
                {
                    Invoke("PlaySFX", i/10);
                }

            }

            cachedCount = count;
        }
    }

    private void PlaySFX()
    {
        AudioManager.Main.RequestSFX(onShootSFX);
    }

    void FixedUpdate()
    {
        ManageSFX();
    }

    public virtual void StopShooting()
    {
        if(singleSFX) AudioManager.Main.StopSFX(sfxInstance);
        shooting = false;
        shooterParticle.Stop(true);
    }

    public virtual void RotateShoots()
    {
        var main = shooterParticle.main;
        main.startRotation = -transform.parent.rotation.z - transform.rotation.z;
    }

    public ParticleSystem GetShooterSystem()
    {
        return shooterParticle;
    }

    public virtual void Update()
    {
        if(transform.parent != null) RotateShoots();
    }

    public abstract void ApplyEffect(HitManager hitManager);

    void OnDestroy()
    {
        // if(AudioManager.Main.IsPlayingSFX(sfxInstance)) AudioManager.Main.StopSFX(sfxInstance);

        if(gameManager != null) gameManager.OnGameStateChange -= ClearShots;
    }

    protected virtual void ApplyStatusEffect<T>(HitManager target, float duration, params float[] parameters) where T : StatusEffect
    {   
        if(target.IsUnderEffect<T>()) return;
        var effect = target.gameObject.AddComponent<T>();
        effect.Initialize(target, duration, parameters);
    }
}