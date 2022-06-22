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
    [SerializeField] protected List<Program> _initialPrograms;
    public List<Program> InitialPrograms { get => _initialPrograms; }
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


    public virtual void Initiate()
    {
        StatSet.ForEach(x => x.Initiate(shooter, this));
        StatSet = StatSet.OrderBy(x => x.sortingIndex).ToList();

        waitForDuration = HasStat<Duration>() ? new WaitForSeconds(GetStatValue<Duration>()) : new WaitForSeconds(GetComponent<Duration>().startingValue);
        waitForCooldown = new WaitForSeconds(GetStatValue<Cooldown>());

        SetInitialEffect();

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleActivation;
        targetSystem = GetComponent<TargetSystem>();
        _animator = GetComponent<Animator>();
        restBar = GetComponentInParent<RestBarManager>();
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

    public bool HasDormentStat(TurretStat T)
    {
        var stat = dormentStats.Find(x => x.GetType() == T.GetType());
        return stat != null;
    }

    public void ExposeDormentStat(TurretStat T)
    {
        var stat = dormentStats.Find(x => x.GetType() == T.GetType());
        if (stat == null) return;
        StatSet.Add(stat);
        dormentStats.Remove(stat);
    }

    public void HideExposedStat(TurretStat T)
    {
        var stat = StatSet.Find(x => x.GetType() == T.GetType());
        if (stat == null) return;
        dormentStats.Add(stat);
        StatSet.Remove(stat);
    }

    public float GetStatValue<T>() where T : TurretStat
    {
        var stat = StatSet.Find(x => x.GetType() == typeof(T));
        return stat.Value;
    }

    public virtual void ApplyDamage(HitManager manager)
    {
        var damage = GetStatValue<Damage>();
        manager.HealthInterface.UpdateHealth(-damage);
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

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();
        var slotId = GetComponentInParent<TurretManager>().slotId;

        container.Add(slotId + "weaponID", BitConverter.GetBytes(Id));

        foreach (TurretStat stat in StatSet)
        {
            var key = stat.publicName + slotId;
            var value = BitConverter.GetBytes(stat.Value);
            container.Add(key, value);
        }

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        var slotId = GetComponentInParent<TurretManager>().slotId;

        foreach (TurretStat stat in StatSet)
        {
            var value = BitConverter.ToSingle(saveFile.GetValue(stat.publicName + slotId));
            stat.SetStatToValue(value);
        }
    }
}
