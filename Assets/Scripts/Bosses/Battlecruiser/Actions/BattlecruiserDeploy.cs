using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiserDeploy : BossAction
{
    [SerializeField] private GameObject deployable;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int quantity;

    [SerializeField] private AnimationCurve launchSpeed;
    [SerializeField] private Vector2 launchDuration;
    [SerializeField] private Vector2 launchAngleVariation;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    
    public override void Action()
    {
    }

    public override void DoActionMove()
    {
    }

    public override void EndAction()
    {
    }

    public override void StartAction()
    {

        for (int i = 0; i < quantity; i++)
        {
            var container = Instantiate(deployable, spawnPoint.position, Quaternion.identity);
            container.GetComponent<BattlecruiserDeployable>().Initiate(transform);

            StartCoroutine(Launch(container));
        }
    }

    private IEnumerator Launch(GameObject cannon)
    {
        var direction = spawnPoint.position - transform.position;
        var _d = ship.position - transform.position;

        direction += _d.normalized;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // var rdm = Random.Range(launchAngleVariation.x, launchAngleVariation.y);

        // angle += rdm;

        // direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        var duration = Random.Range(launchDuration.x, launchDuration.y);
        float step = 0;

        cannon.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        while(step <= duration)
        {
            cannon.transform.position += direction.normalized * launchSpeed.Evaluate(step / duration);
            step += 0.01f;
            yield return waitTime;
        }

        cannon.GetComponent<BattlecruiserDeployable>().LateInitiate();
    }
}
