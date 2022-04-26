using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowerController : ActionController
{
    [SerializeField] private float maxAngleVariation = 60;
    private bool seeking;


    void Update()
    {
        if(enemiesInSight.Count > 0 && !seeking)
        {
            GetTarget();
        }
    }

    protected void GetTarget()
    {
        // yield return new WaitUntil(() => enemiesInSight.Count > 0);
        target = enemiesInSight.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
            .FirstOrDefault();

        target.OnDetach += DetachTarget;

        foreach(ActionEffect shooter in shooters)
        {
            shooter.ReceiveTarget(target.gameObject);
        }

        seeking = true;

        StartCoroutine(FollowTarget(target));

    }

    private void DetachTarget(object sender, EventArgs e)
    {
        var _t = (TargetableComponent)sender;
        enemiesInSight.Remove(_t);
        target = null;
        _t.OnDetach -= DetachTarget;
    }

    protected IEnumerator FollowTarget(TargetableComponent target)
    {
        
        StartCoroutine(ManageActivation());

        while(target != null)
        {
            Vector3 direction = target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            this.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            if(CheckRotation()) break;

            yield return new WaitForEndOfFrame();
        }

        StopCoroutine(ManageActivation());
        
        StopShooters();

        StartCoroutine(ReturnToInitialRotation());
    }


    private bool CheckRotation()
    {
        if(transform.localRotation.eulerAngles.z > maxAngleVariation && transform.localRotation.eulerAngles.z < 360 - maxAngleVariation) return true;
        return false;
    }

    protected override IEnumerator ManageActivation()
    {
        while(true)
        {        
            Activate();

            float duration = shooters[0].GetShooterSystem().main.duration;

            yield return new WaitForSeconds(duration);    
            // StopShooters();
   
        }

    }

    
    public override void Activate()
    {
        foreach(ActionEffect shooter in shooters)
        {
            if(shooter.GetShooterSystem().IsAlive()) return;
            shooter.Shoot();
        }
    }

    private IEnumerator ReturnToInitialRotation()
    {

        while((int)transform.localRotation.eulerAngles.z != 0)
        {
            float sign = Mathf.Sign(transform.localRotation.eulerAngles.z - 180);
            transform.Rotate(0, 0, 1f * sign, Space.Self);
            yield return new WaitForSeconds(.01f);
        }

        seeking = false;
    }

}
