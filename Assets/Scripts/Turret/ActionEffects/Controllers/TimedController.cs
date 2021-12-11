using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedController : ActionController
{
    [SerializeField] private float timeBetweenActivations;
    private WaitForSecondsRealtime wait;

    void Awake()
    {
        wait = new WaitForSecondsRealtime(timeBetweenActivations);

        StartCoroutine(ManageActivation());
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
}
