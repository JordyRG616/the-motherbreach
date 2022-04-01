using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallAction : BossAction
{
    [SerializeField] private Transform recallPosition;
    [Header("Effects")]
    [SerializeField] private ParticleSystem trackBeam;
    private ForgeController forgeController;

    void Start()
    {
        forgeController = GetComponent<ForgeController>();
    }

    

    private IEnumerator Recall(Transform formationHead)
    {
        var formation = formationHead.parent.GetComponent<FormationManager>();

        foreach(Rigidbody2D body in formation.GetComponentsInChildren<Rigidbody2D>())
        {
            body.Sleep();
        }

        var position = formationHead.position;
        float step = 0;

        trackBeam.transform.position = position;
        trackBeam.Play();

        while(step <= 1)
        {
            if(formationHead == null) StopAllCoroutines();
            var newPos = Vector2.Lerp(position, recallPosition.position, step);
            formationHead.position = newPos;
            trackBeam.transform.position = formationHead.position;
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        trackBeam.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        yield return new WaitForSeconds(0.5f);

        foreach(EnemyManager enemy in formation.Children)
        {
            enemy.AdjustLevel(enemy.level + 1);
            enemy.GetComponent<Rigidbody2D>().WakeUp();
        }

    }

    public override void StartAction()
    {
        if(forgeController.Children.Count == 0) return;
        var rdm = UnityEngine.Random.Range(0, forgeController.Children.Count);
        var head = forgeController.Children[rdm].transform.Find("Head");
        StartCoroutine(Recall(head));
    }

    public override void Action()
    {

    }

    public override void EndAction()
    {

    }

    public override void DoActionMove()
    {

    }
}
