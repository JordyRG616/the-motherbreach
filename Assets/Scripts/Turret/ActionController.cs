using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionController : MonoBehaviour
{
    private TopActionTemplate action;
    [SerializeField] private List<EnemyManager> enemiesInSight = new List<EnemyManager>();
    [SerializeField] private float maxAngle, searchSpeed;
    private float angle = 0;

    public void Initialize(TopActionTemplate action, float cooldown)
    {
        this.action = action;
        InvokeRepeating("Activate", cooldown, cooldown);

        StartCoroutine(SearchForTarget());
    }

    private void Activate()
    {
        EnemyManager target = GetTarget();
        if(target != null)
        {
            action.ActivateAction(target.gameObject);
        }
    }

    private EnemyManager GetTarget()
    {
        if(enemiesInSight.Count > 0)
        {
            StopAllCoroutines();
            enemiesInSight.OrderBy( x => Vector3.Distance(this.transform.position, x.transform.position));
            return enemiesInSight.FirstOrDefault(); 
        }
        else
        {
            return null;
        }
    }

    private IEnumerator SearchForTarget()
    {
        while(enemiesInSight.Count == 0)
        {
            transform.Rotate(new Vector3(0, 0, .1f * searchSpeed));

            angle += .1f * searchSpeed;
            if(angle >= maxAngle || angle <= -maxAngle)
            {
                searchSpeed *= -1;
            }

            yield return new WaitForSecondsRealtime(.1f);         
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyManager>(out EnemyManager enemy))
        {
            enemiesInSight.Add(enemy);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyManager>(out EnemyManager enemy))
        {
            if(enemiesInSight.Contains(enemy))
            {
                enemiesInSight.Remove(enemy);
                
                if(enemiesInSight.Count == 0)
                {
                    StartCoroutine(SearchForTarget());
                }
            }
        }
    }
}
