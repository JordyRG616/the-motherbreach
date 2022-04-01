using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportController : ActionController
{
    [SerializeField] private float timeBetweenActivations;
    [SerializeField] private LayerMask playerLayer;
    private List<IntegrityManager> turrets = new List<IntegrityManager>();
    private WaitForSeconds wait;
    private GameManager gameManager;

    void OnEnable()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleActivation;

        wait = new WaitForSeconds(timeBetweenActivations);
    }

    private void HandleActivation(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward) StopAllCoroutines();
        if(e.newState == GameState.OnWave) StartCoroutine(ManageActivation());
    }

    protected override IEnumerator ManageActivation()
    {
        while(true)
        {
            // yield return wait;

            Activate();

            float duration = 0;
            if(shooters[0].StatSet.ContainsKey(Stat.Duration)) duration = shooters[0].StatSet[Stat.Duration];

            yield return new WaitForSeconds(duration);

            SetOnRest();

            yield return new WaitForSeconds(shooters[0].StatSet[Stat.Rest]);
        }
    }

    public override void Activate()
    {
        foreach(ActionEffect shooter in shooters)
        {
            var target = GetTarget();
            shooter.Shoot();
        }
    }

    public IntegrityManager GetTarget()
    {
        GetNeighbouringTurrets();
        if(turrets.Count == 0) return null;
        var target = turrets.OrderBy(x => x.GetCurrentIntegrity());
        return target.First();
    }

    void OnDisable()
    {
        if(gameManager !=null) gameManager.OnGameStateChange -= HandleActivation;
    }

    private void GetNeighbouringTurrets()
    {
        turrets.Clear();
        var targets = Physics2D.CircleCastAll(transform.position, 4.5f, Vector2.up, 0, playerLayer);
        foreach(RaycastHit2D target in targets)
        {
            if(target.collider.TryGetComponent<IntegrityManager>(out var integrityManager))
            {
                turrets.Add(integrityManager);
            }
        }
        turrets.Remove(GetComponentInParent<IntegrityManager>());
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        // if(other.TryGetComponent<IntegrityManager>(out var integrityManager))
        // {
        //     turrets.Add(integrityManager);
        // }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        // if(other.TryGetComponent<IntegrityManager>(out var integrityManager))
        // {
        //     if(turrets.Contains(integrityManager))
        //     {
        //         turrets.Remove(integrityManager);
        //     }
        // }
    }
}
