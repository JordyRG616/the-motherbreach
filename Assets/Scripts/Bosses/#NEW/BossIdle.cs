using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* It's a class that makes the boss move around the screen */
public abstract class BossIdle : MonoBehaviour
{
    [SerializeField] protected float speed;
    
    public bool paused;
    protected Rigidbody2D body;
    protected Transform ship;
    protected BossController controller;
    private float refSpeed;

    protected virtual void Start()
    {
        body = GetComponent<Rigidbody2D>();
        controller = GetComponent<BossController>();
        ship = ShipManager.Main.transform;
    }    

    public abstract void IdleMove();

    /// <summary>
    /// This function takes a Vector2 direction and rotates the object to face that direction
    /// </summary>
    /// <param name="direction">The direction you want the object to look at.</param>
    protected void LookAt(Vector2 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}
