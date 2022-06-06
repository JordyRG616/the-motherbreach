using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedController : ActionController
{
    [SerializeField] private float timeBetweenActivations;
    private WaitForSeconds wait;
    private WaitUntil until;

    void OnEnable()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleActivation;

        wait = new WaitForSeconds(timeBetweenActivations);
        until = new WaitUntil(() => shooters[0].GetShooterSystem().isEmitting == false);
    }

    private void HandleActivation(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward) StopAllCoroutines();
        if(e.newState == GameState.OnWave) StartCoroutine(ManageActivation());
    }

    protected override IEnumerator ManageActivation()
    {
        SetOnRest();

        while (true)
        {

            yield return new WaitForSeconds(shooters[0].StatSet[Stat.Rest]);

            Activate();

            yield return until;

            SetOnRest();
        }
    }

    public override void Activate()
    {
        foreach(ActionEffect shooter in shooters)
        {
            shooter.ReceiveTarget(shooter.gameObject);
            shooter.Shoot();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if(gameManager !=null) gameManager.OnGameStateChange -= HandleActivation;
    }
}
