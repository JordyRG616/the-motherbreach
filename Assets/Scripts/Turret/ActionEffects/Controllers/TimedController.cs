using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedController : ActionController
{
    [SerializeField] private float timeBetweenActivations;
    private WaitForSecondsRealtime wait;
    private GameManager gameManager;

    void OnEnable()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HandleActivation;

        wait = new WaitForSecondsRealtime(timeBetweenActivations);
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
            yield return wait;

            Activate();

            yield return new WaitForSecondsRealtime(shooters[0].StatSet[Stat.Rest]);
        }
    }

    public override void Activate()
    {
        foreach(ActionEffect shooter in shooters)
        {
            shooter.Shoot();
        }
    }

    void OnDisable()
    {
        if(gameManager !=null) gameManager.OnGameStateChange -= HandleActivation;
    }
}
