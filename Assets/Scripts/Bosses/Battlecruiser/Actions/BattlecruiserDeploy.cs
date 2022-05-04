using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiserDeploy : BossAction
{
    [SerializeField] private GameObject deployable;
    [SerializeField] private int quantity;
    [SerializeField] private float launchSpeed;

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
        var angleIncrement = 360 / quantity;
        for (int i = 0; i < quantity; i++)
        {
            var container = Instantiate(deployable, transform.position, Quaternion.identity);
            container.GetComponent<BattlecruiserDeployable>().Initiate(transform);

            var angle = i * angleIncrement;
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            StartCoroutine(Launch(container.transform, direction));
        }
    }

    private IEnumerator Launch(Transform deployable, Vector3 direction)
    {
        float step = 0;

        while(step <= 1)
        {
            deployable.transform.position += launchSpeed * direction;
            step += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
