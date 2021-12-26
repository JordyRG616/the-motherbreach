using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployRogueState : FormationState
{
    [SerializeField] private float rogueSpeed;
    [SerializeField] private float rogueOrbitDistance;
    protected override void HandleChildren(EnemyManager[] children, float step)
    {
        int rdm;
        EnemyManager enemy;
        do{
            rdm = Random.Range(0, children.Length);
            enemy = children[rdm];
        } while(children[rdm].transform.parent != this.transform);

        enemy.transform.parent = null;
        enemy.GetComponent<MovableEntity>().Anchor();
        enemy.GetComponent<EnemyRotationController>().StopRotation();
        enemy.gameObject.AddComponent<RogueController>().Initialize(rogueSpeed, rogueOrbitDistance);
    }

    protected override void OnStateTick(float step)
    {
    }
}
