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

    private IEnumerator ManageActivation()
    {
        while(true)
        {
            yield return wait;

            Activate();
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
