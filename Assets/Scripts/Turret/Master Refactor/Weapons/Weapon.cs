using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour, ISavable
{
    [SerializeField] protected int _id;
    public int Id { get => _id; }
    [SerializeField] protected int _cost;
    public int Cost { get => _cost; }
    [SerializeField] protected WeaponClass _class;
    public WeaponClass Class { get => _class; }
    [SerializeField] [TextArea] protected string _description;
    public string Description { get => _description; }
    [SerializeField] protected List<TurretStat> StatSet;
    [SerializeField] protected List<TurretStat> dormentStats;
    [SerializeField] protected List<Trait> _initialPrograms;
    public List<Trait> InitialPrograms { get => _initialPrograms; }
    [SerializeField] protected ParticleSystem shooter;
    public WaitForSeconds waitForCooldown;
    public WaitForSeconds waitForDuration;

    public delegate void WeaponEffect(HitManager manager);
    public WeaponEffect totalEffect;

    protected GameManager gameManager;
    protected TargetSystem targetSystem;
    protected Animator _animator;
    protected RestBarManager restBar;
    protected float restCounter;
    public bool attacking { get; protected set; }
    protected bool initiated;


    public virtual void Initiate()
    {
        if (initiated) return;
        StatSet.ForEach(x => x.Initiate(shooter, this));
        StatSet.ForEach(x => x.overwritten = true);
        dormentStats.ForEach(x => x.Initiate(shooter, this));
        StatSet = StatSet.OrderBy(x => x.sortingIndex).ToList();
        dormentStats = dormentStats.OrderBy(x => x.sortingIndex).ToList();

        var duration = HasStat<Duration>() ? GetStatValue<Duration>() : shooter.main.duration;

        waitForDuration = HasStat<Duration>() ? new WaitForSeconds(duration) : new WaitForSeconds(duration);
        waitForCooldown = new WaitForSeconds(GetStatValue<Cooldown>());

        SetInitialEffect();

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleActivation;
        targetSystem = GetComponent<TargetSystem>();
        _animator = GetComponent<Animator>();
        restBar = GetComponentInParent<RestBarManager>();
        initiated = true;
    }

    protected virtual void HandleActivation(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnWave)
        {
            StartCoroutine(ManageActivation());
        } else
        {
            StopCoroutine(ManageActivation());
        }
    }

    protected virtual void Update()
    {
        if (gameManager.gameState != GameState.OnWave) return;
        restCounter += Time.deltaTime;
        var cooldown = GetStatValue<Cooldown>();
        if (restCounter > cooldown) restCounter = cooldown;
        restBar.SetBarPercentual(restCounter / cooldown);
    }

    protected abstract void SetInitialEffect();

    public bool HasStat(TurretStat T)
    {
        var stat = StatSet.Find(x => x.GetType() == T.GetType());
        return stat != null;
    }
    public bool HasStat<T>() where T : TurretStat
    {
        var stat = StatSet.Find(x => x.GetType() == typeof(T));
        return stat != null;
    }

    public bool HasStat(TurretStat T, out TurretStat stat)
    {
        stat = StatSet.Find(x => x.GetType() == T.GetType());
        return stat != null;
    }

    public bool HasStat<T>(out TurretStat stat) where T : TurretStat
    {
        stat = StatSet.Find(x => x.GetType() == typeof(T));
        return stat != null;
    }

    public bool HasDormentStat(TurretStat T)
    {
        //var stat = dormentStats.Find(x => x.GetType().IsSubclassOf(T.GetType()));
        var type = T.GetType();
        foreach (var testedStat in dormentStats)
        {
            var _t = testedStat.GetType();
            if (_t == type || _t.IsSubclassOf(type)) return true;
        }
        return false;
    }

    public void ExposeDormentStat(TurretStat T)
    {
        //var stat = dormentStats.Find(x => x.GetType() == T.GetType());
        TurretStat stat = null;
        var type = T.GetType();
        foreach (var _stat in dormentStats)
        {
            var _t = _stat.GetType();
            if (_t == type || _t.IsSubclassOf(type)) stat = _stat;
        }
        if (stat == null) return;
        StatSet.Add(stat);
        dormentStats.Remove(stat);
        stat.Initiate(shooter, this);
        stat.overwritten = true;
    }

    public void HideExposedStat(TurretStat T)
    {
        TurretStat stat = null;
        var type = T.GetType();
        foreach (var _stat in StatSet)
        {
            var _t = _stat.GetType();
            if (_t == type || _t.IsSubclassOf(type)) stat = _stat;
        }
        if (stat == null) return;
        dormentStats.Add(stat);
        StatSet.Remove(stat);
        stat.overwritten = false;
    }

    public float GetStatValue<T>() where T : TurretStat
    {
        //var stat = StatSet.Find(x => x.GetType() == typeof(T));
        TurretStat stat = null;
        var type = typeof(T);
        foreach (var _stat in StatSet)
        {
            var _t = _stat.GetType();
            if (_t == type || _t.IsSubclassOf(type)) stat = _stat;
        }

        if(stat == null)
        {
            foreach (var _stat in dormentStats)
            {
                var _t = _stat.GetType();
                if (_t == type || _t.IsSubclassOf(type)) stat = _stat;
            }
        }

        stat.Initiate(shooter, this);
        return stat.Value;
    }

    public virtual void ApplyDamage(HitManager manager)
    {
        var damage = GetStatValue<Damage>();
        manager.HealthInterface.UpdateHealth(-damage);
    }

    private void OnDestroy()
    {
        if (!initiated) return;
        gameManager.OnGameStateChange -= HandleActivation;
    }

    protected virtual IEnumerator ManageActivation()
    {
        while(true)
        {
            yield return waitForCooldown;
            yield return new WaitUntil(() => HasTarget());

            OpenFire();

            yield return waitForDuration;

            CeaseFire();
        }
    }

    protected virtual bool HasTarget()
    {
        return targetSystem.target != null;
    }

    protected virtual void OpenFire()
    {
        shooter.Play();
        attacking = true;
        _animator.SetBool("Attacking", true);
    }

    protected virtual void CeaseFire()
    {
        shooter.Stop(true);
        attacking = false;
        restCounter = 0;
        _animator.SetBool("Attacking", false);
    }

    public List<TurretStat> GetTurretStats()
    {
        return StatSet;
    }

    public List<TurretStat> GetDormentStats()
    {
        return dormentStats;
    }

    public List<TurretStat> GetAllStats()
    {
        var list = new List<TurretStat>();
        list.AddRange(StatSet);
        list.AddRange(dormentStats);
        return list;
    }

    public int GetActiveStatCount()
    {
        return StatSet.Count;
    }

    public int GetDormentStatCount()
    {
        return dormentStats.Count;
    }

    public void ReplaceShooter(ParticleSystem newShooter)
    {
        shooter = newShooter;
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();
        var slotId = GetComponentInParent<TurretManager>().slotId;

        container.Add(slotId + "weaponID", BitConverter.GetBytes(Id));

        var list = new List<TurretStat>();
        list.AddRange(StatSet);
        list.AddRange(dormentStats);

        foreach (TurretStat stat in list)
        {
            var key = stat.publicName + slotId;
            var value = BitConverter.GetBytes(stat.Value);
            container.Add(key, value);
        }

        return container;
    }

    public virtual void LoadData(SaveFile saveFile)
    {
        var slotId = GetComponentInParent<TurretManager>().slotId;

        var list = new List<TurretStat>();
        list.AddRange(StatSet);
        list.AddRange(dormentStats);

        list.ForEach(x => x.Initiate(shooter, this));

        foreach (TurretStat stat in list)
        {
            var value = BitConverter.ToSingle(saveFile.GetValue(stat.publicName + slotId));
            stat.SetStatToValue(value);
        }
    }
}
