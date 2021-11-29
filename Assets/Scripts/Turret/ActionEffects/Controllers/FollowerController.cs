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
        Debug.Log("target acquired");
        yield return new WaitUntil(() => enemiesInSight.Count > 0);
        target = enemiesInSight.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
            .FirstOrDefault();

        foreach(ActionEffect shooter in shooters)
        {
            shooter.ReceiveTarget(target.gameObject);
        }

        StartCoroutine(FollowTarget(target));
        Activate();
    }

    protected IEnumerator FollowTarget(EnemyManager target)
    {
        Debug.Log("following target");
        StopCoroutine(GetTarget());

        while(enemiesInSight.Contains(target))
        {
            Vector3 direction = target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            foreach(ActionEffect shooter in shooters)
            {
                shooter.RotateShoots(-angle + 90f);
            }

            yield return new WaitForEndOfFrame();
        }
    
        StopShooters();

        StartCoroutine(ReturnToInitialRotation());
    }

    
    public override void Activate()
    {
        for(int i = 0; i < shooters.Count;i++ )
        {
            shooters[i].Invoke("Shoot", shooters[i].GetData().Cooldown / shooters.Count * i);
        }
    }

    private IEnumerator ReturnToInitialRotation()
    {
        Debug.Log("stop following");
        while((int)transform.localRotation.eulerAngles.z != 0)
        {
            float sign = Mathf.Sign(transform.localRotation.eulerAngles.z - 180);
            transform.Rotate(0, 0, 1f * sign, Space.Self);
            yield return new WaitForSecondsRealtime(.01f);
        }

        StartCoroutine(GetTarget());
    }
}
