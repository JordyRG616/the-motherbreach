using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormationMovement : MonoBehaviour
{
    protected FormationManager formationMovement;
    protected List<EnemyManager> enemies = new List<EnemyManager>();
    public abstract EnemyMovementType Type {get; protected set;}
    public float speedModifier;
    private float _speedModifier
    {
        get
        {
            return 1 + speedModifier;
        }
    }

    protected virtual void Start()
    {
        formationMovement = GetComponentInParent<FormationManager>();
    }

    public virtual void Initiate(){}

    protected virtual void FixedUpdate()
    {
        enemies = formationMovement.Children;
    }

    protected virtual void RotateChildren(Vector2 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        foreach(EnemyManager enemy in enemies)
        {
            enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    protected virtual void FixChildren()
    {
        foreach(EnemyManager enemy in enemies)
        {
            enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    protected virtual void RotateChildren(Vector2 direction, float angleVariation)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var sign = 1;

        foreach(EnemyManager enemy in enemies)
        {
            enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90f + (sign * angleVariation));
            sign *= -1;
        }
    }

    public abstract void DoMovement(Rigidbody2D body, Vector3 target);
}

[System.Flags]
public enum EnemyMovementType 
    {
        None = 0, 
        Fixed = 1, 
        Flee = 2, 
        Orbit = 4,
        Split = 8,
        TrueOrbit = 16,
        Cone = 32
    }