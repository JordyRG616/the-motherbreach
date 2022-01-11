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

        foreach(ActionEffect shooter in shooters)
        {
            shooter.ReceiveTarget(target.gameObject);
        }

        seeking = true;

        StartCoroutine(FollowTarget(target));
    }

    protected IEnumerator FollowTarget(EnemyManager target)
    {
        StartCoroutine(ManageActivation());

        while(enemiesInSight.Contains(target))
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

            yield return new WaitForSeconds(shooters[0].StatSet[Stat.Rest]);
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
            yield return new WaitForSeconds(.01f);
        }

        seeking = false;
    }
}
