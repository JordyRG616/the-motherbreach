using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueOrbitMovement : FormationMovement
{
    [SerializeField] private float speed;
    [SerializeField] private bool fixedRotation;

    public override EnemyMovementType Type { get; protected set; } = EnemyMovementType.TrueOrbit;

    public override void DoMovement(Rigidbody2D body, Vector3 target)
    {
        body.velocity = Vector2.zero;

        if(fixedRotation) return;  

        foreach(EnemyManager enemy in enemies)
        {
            if(enemy == null) continue;
            var distance = (transform.position - enemy.transform.position);
            // distance = Vector2.Perpendicular(distance);
            enemy.GetComponent<Rigidbody2D>().AddForce(distance.normalized * 10 * speed);     

            var angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            
        }
        // body.AddForce(finalDirection * speed * 100);
    }

}
