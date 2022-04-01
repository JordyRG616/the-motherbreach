using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossIdle : MonoBehaviour
{
    [SerializeField] protected float speed;
    public bool paused;
    protected Rigidbody2D body;
    protected Transform ship;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        ship = ShipManager.Main.transform;
    }    

    public abstract void IdleMove();

    protected void LookAt(Vector2 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}
