using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedController : ActionController
{
    [SerializeField] private float timeBetweenActivations;
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
            if (shooters[0].StatSet.ContainsKey(Stat.Duration)) duration = GetAttackDuration();

            yield return new WaitForSeconds(duration);

            SetOnRest();

            yield return new WaitForSeconds(shooters[0].StatSet[Stat.Rest]);
        }
    }

    private float GetAttackDuration()
    {
        return shooters[0].StatSet[Stat.Duration] + shooters[0].GetShooterSystem().main.startLifetime.constant;
    }

    public override void Activate()
    {
        foreach(ActionEffect shooter in shooters)
        {
            shooter.ReceiveTarget(shooter.gameObject);
            shooter.Shoot();
        }
    }

    void OnDisable()
    {
        if(gameManager !=null) gameManager.OnGameStateChange -= HandleActivation;
    }
}
