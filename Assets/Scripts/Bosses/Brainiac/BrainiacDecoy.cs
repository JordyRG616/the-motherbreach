using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainiacDecoy : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    private float count;
    private Transform ship;
    private Rigidbody2D body;

    void Start()
    {
        ship = ShipManager.Main.transform;
        body = GetComponent<Rigidbody2D>();
        var _p = Instantiate(pointer, transform.position, Quaternion.identity);
        _p.GetComponent<EnemyPointer>().ReceiveTarget(transform);
    }

    void FixedUpdate()
    {
        var direction = ship.position - transform.position;
        LookAt(direction);

        var _d = Random.onUnitSphere;
        
        body.AddForce(_d * speed);

        count += Time.fixedDeltaTime;
        if(count >= duration) Destroy(gameObject);

    }

    protected void LookAt(Vector2 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}
