using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowerController : ActionController
{
    void Awake()
    {
        StartCoroutine(GetTarget());
    }

    protected IEnumerator GetTarget()
    {
        yield return new WaitUntil(() => enemiesInSight.Count > 0);
        target = enemiesInSight.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
            .FirstOrDefault();

        foreach(ActionEffect shooter in shooters)
        {
            shooter.ReceiveTarget(target.gameObject);
        }

        StartCoroutine(FollowTarget(target));
    }

    protected IEnumerator FollowTarget(EnemyManager target)
    {
        StopCoroutine(GetTarget());

        var _manager = ManageActivation();

        StartCoroutine(_manager);

        while(enemiesInSight.Contains(target))
        {
            Vector3 direction = target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            yield return new WaitForEndOfFrame();
        }

        StopCoroutine(_manager);
        
        StopShooters();

        StartCoroutine(ReturnToInitialRotation());
    }

    protected override IEnumerator ManageActivation()
    {
        while(true)
        {
            Activate();

            yield return new WaitForSecondsRealtime(shooters[0].StatSet[ActionStat.Rest]);
        }

    }

    
    public override void Activate()
    {
        for(int i = 0; i < shooters.Count;i++ )
        {
            shooters[i].Invoke("Shoot", .5f * i);
        }
    }

    private IEnumerator ReturnToInitialRotation()
    {
        while((int)transform.localRotation.eulerAngles.z != 0)
        {
            float sign = Mathf.Sign(transform.localRotation.eulerAngles.z - 180);
            transform.Rotate(0, 0, 1f * sign, Space.Self);
            yield return new WaitForSecondsRealtime(.01f);
        }

        StartCoroutine(GetTarget());
    }
}
