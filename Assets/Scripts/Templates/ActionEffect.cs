using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using StringHandler;

public abstract class ActionEffect : MonoBehaviour, ISavable
{

    [SerializeField] protected ParticleSystem shooterParticle;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Keyword keyword;
    public WeaponTag tags;
    protected GameObject target;

    [Header("Initial basic stats")]
    [SerializeField] protected float initialDamage;
    [SerializeField] protected float initialRest;
    
    public Dictionary<Stat, float> StatSet {get; protected set;} = new Dictionary<Stat, float>();
    public Dictionary<Stat, float> cachedStatSet = new Dictionary<Stat, float>();
    public abstract Stat specializedStat {get;}
    public abstract Stat secondaryStat {get;}
    [Header("Descriptions")]
    public string damageText;
    public string specializedStatText;
    public string secondaryStatText;

    protected AudioManager audioManager;
    protected GameManager gameManager;
    [SerializeField] [FMODUnity.EventRef] protected string onShootSFX;
    protected List<FMOD.Studio.EventInstance> sfxInstances = new List<FMOD.Studio.EventInstance>();

    public delegate void Effect(HitManager hitManager);

    public Effect totalEffect;
    [SerializeField] protected bool singleSFX;
    protected int cachedCount = 0;
    protected bool onRest;
    protected float cooldown;
    [HideInInspector] public float initialRotation;
    private bool initiated;

    [Header("Debug")]
    public string[] debugStats = new string[0];

    public virtual void Initiate()
    {
        if(initiated) return;
        shooterParticle.Stop(true);

        SetData();

        totalEffect += ApplyEffect;

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += ClearShots;

        audioManager = AudioManager.Main;

        initiated = true;
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
    }

    public abstract void LevelUp(int toLevel);

    public virtual void SetData()
    {
        StatSet.Add(Stat.Damage, initialDamage);
        StatSet.Add(Stat.Rest, initialRest);

        var col = shooterParticle.collision;
        col.collidesWith = targetLayer;
    }

    public virtual void SetStat(Stat statName, float value)
    {
        if(StatSet.ContainsKey(statName))
        {
            StatSet[statName] = value;
            StatSet[statName] = (float)Math.Round(StatSet[statName], 1);
        } 
    }

    public virtual void ReceiveTarget(GameObject parentTarget)
    {
        target = parentTarget;
    }

    public virtual void Shoot()
    {
        if(onRest) return;
        if(target != null && !shooterParticle.isEmitting)
        {
            if(singleSFX) 
            {
                sfxInstances.ForEach(x => StopSFX(x));
                sfxInstances.Clear();
                audioManager.RequestSFX(onShootSFX, out var sfxInstance);
                sfxInstances.Add(sfxInstance);
            }
            shooterParticle.Play(true);
        }
    }

    protected virtual void ManageSFX()
    {
        if(!singleSFX && shooterParticle.isPlaying)
        {
            var amount = Mathf.Abs(cachedCount - shooterParticle.particleCount);

            if (shooterParticle.particleCount > cachedCount) 
            { 
                PlaySFX();
            } 

            cachedCount = shooterParticle.particleCount;
        }
    }

    protected virtual void PlaySFX()
    {
        audioManager.RequestSFX(onShootSFX);
    }

    protected virtual void StopSFX(FMOD.Studio.EventInstance audio)
    {
        audioManager.StopSFX(audio);
    }

    void LateUpdate()
    {
        ManageSFX();
    }

    public virtual void StopShooting()
    {
        if(singleSFX)
        {
            sfxInstances.ForEach(x => StopSFX(x));
            sfxInstances.Clear();
        }
        shooterParticle.Stop(true);
        onRest = true;
        cooldown = 0;
    }

    public virtual void RotateShoots()
    {
        var main = shooterParticle.main;
        var parent = transform.parent;
        main.startRotation =(-initialRotation - ShipManager.Main.transform.eulerAngles.z - transform.localEulerAngles.z) * Mathf.Deg2Rad;
    }

    public ParticleSystem GetShooterSystem()
    {
        return shooterParticle;
    }

    public virtual void Update()
    {
        if(onRest)
        {
            cooldown += Time.deltaTime;
            if(cooldown >= StatSet[Stat.Rest]) onRest = false;
        }
        if(GameManager.Main.gameState == GameState.OnWave) RotateShoots();
        #if UNITY_EDITOR
            UpdateDebugStats();
        #endif
    }

    private void UpdateDebugStats()
    {
        var stats = StatSet.Keys.ToList();
        if(debugStats.Length <= stats.Count)
        {
            debugStats = new string[stats.Count];
        }
        for(int i = 0; i < stats.Count; i++)
        {
            debugStats[i] = (stats[i] + ": " + StatSet[stats[i]]);
        }
    }

    public abstract void ApplyEffect(HitManager hitManager);

    void OnDestroy()
    {
        if(gameManager != null) gameManager.OnGameStateChange -= ClearShots;
    }

    protected virtual void ApplyStatusEffect<T>(HitManager target, float duration, params float[] parameters) where T : StatusEffect
    {   
        if(target.IsUnderEffect<T>(out var status)) status.DestroyEffect();
        var effect = target.gameObject.AddComponent<T>();
        effect.Initialize(target, duration, parameters);
    }

    public void RememberStatSet()
    {
        cachedStatSet.Clear();
        foreach(Stat stat in StatSet.Keys)
        {
            cachedStatSet.Add(stat, StatSet[stat]);
        }
    }

    public void ResetStatSet()
    {
        foreach(Stat stat in cachedStatSet.Keys)
        {
            StatSet[stat] = cachedStatSet[stat];
        }
    }

    public float GetRestPercentual()
    {
        if(!StatSet.ContainsKey(Stat.Rest)) return 0;
        return cooldown / StatSet[Stat.Rest];
    }

    public void SetToRest()
    {
        cooldown = 0;
        onRest = true;
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();

        foreach(Stat stat in StatSet.Keys)
        {
            var key = stat.ToString();
            var value = BitConverter.GetBytes(StatSet[stat]);
            container.Add(key, value);
        }

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        
    }

}