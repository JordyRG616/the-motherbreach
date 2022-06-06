using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiserController : BossController
{
    [SerializeField] private BossAction deployAction;

    void Start()
    {
        deployAction.StartAction();
    }

    protected override void SecondPhaseUpgrade()
    {
        deployAction.StartAction();
    }

    protected override void ThirdPhaseUpgrade()
    {
        deployAction.StartAction();
    }
}
