using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeController : BossController
{
    [SerializeField] private int maxChildCount;
    public List<FormationManager> Children {get; private set;} = new List<FormationManager>();
 
    protected override void Awake()
    {
        AudioManager.Main.RequestBossMusic(bossMusic, out musicInstance);

        healthController = GetComponent<BossHealthController>();
        healthController.Initiate();
        animator = GetComponentInChildren<Animator>();
        ship = ShipManager.Main.transform;

        waitTime = new WaitForSeconds(intervalToCheck);

        movement = idle.IdleMove;

        StartCoroutine(ManageActions());

        phases.ForEach(x => x.Initiate());
    }
    protected override void ThirdPhaseUpgrade()
    {
        intervalToCheck -= .3f;
        maxChildCount += 3;
    }

    protected override void SecondPhaseUpgrade()
    {
        intervalToCheck -= .2f;
        maxChildCount += 2;
    }

    public bool HasMaxCapacity()
    {
        return Children.Count >= maxChildCount;
    }
}
