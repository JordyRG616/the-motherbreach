using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected List<TurretStat> StatSet;
    [SerializeField] protected ParticleSystem shooter;
    public WaitForSeconds waitForCooldown;
    public WaitForSeconds waitForDuration;

    public delegate void WeaponEffect(HitManager manager);
    public WeaponEffect totalEffect;

    private GameManager gameManager;

    public virtual void Initiate()
    {
        StatSet.ForEach(x => x.Initiate(shooter, this));

        waitForDuration = new WaitForSeconds(GetStatValue<Duration>());
        waitForCooldown = new WaitForSeconds(GetStatValue<Cooldown>());

        SetInitialEffect();

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleActivation;
    }

    private void HandleActivation(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnWave)
        {
            StartCoroutine(ManageActivation());
        } else
        {
            StopCoroutine(ManageActivation());
        }
    }

    protected abstract void SetInitialEffect();

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

            OpenFire();

            yield return waitForDuration;

            CeaseFire();
        }
    }

    protected virtual void OpenFire()
    {
        shooter.Play();
    }

    protected virtual void CeaseFire()
    {
        shooter.Stop();
    }
}
