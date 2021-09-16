using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ActionController : MonoBehaviour
{
    [SerializeField] protected List<ActionEffect> action;
    protected List<EnemyManager> enemiesInSight = new List<EnemyManager>();
    protected EnemyManager target;

    private IEnumerator GetTarget()
    {
        yield return new WaitUntil(() => enemiesInSight.Count > 0);
        target = enemiesInSight.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
            .FirstOrDefault();
        StartCoroutine(FollowTarget(target));
        Activate();
        StopCoroutine(GetTarget());
    }

    public abstract void Activate();

    protected IEnumerator FollowTarget(EnemyManager target)
    {
        while(enemiesInSight.Contains(target))
        {
            Vector3 direction = target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return new WaitForEndOfFrame();
        }

        StopCoroutine("FollowTarget");
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyManager>(out EnemyManager enemy))
        {
            enemiesInSight.Add(enemy);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyManager>(out EnemyManager enemy))
        {
            if(enemiesInSight.Contains(enemy))
            {
                enemiesInSight.Remove(enemy);
            }
        }
    }
}
