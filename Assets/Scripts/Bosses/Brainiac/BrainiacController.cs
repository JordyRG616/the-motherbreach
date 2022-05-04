using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainiacController : BossController
{
    protected override void SecondPhaseUpgrade()
    {
        GetComponent<TeleportAction>().deployDecoy = true;
        GetComponent<BulletFieldAction>().actionDuration += 1f;
        intervalToCheck -= .15f;
    }

    protected override void ThirdPhaseUpgrade()
    {
        intervalToCheck -= .15f;
        GetComponent<BulletFieldAction>().actionDuration += 1.5f;
    }
}
