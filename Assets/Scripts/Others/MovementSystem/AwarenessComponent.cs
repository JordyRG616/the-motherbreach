using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessComponent : MonoBehaviour
{
    [SerializeField] private float radius;
    private MovableEntity movableEntity;
    [SerializeField] private LayerMask contactLayer;
    [SerializeField] private float strength;


    void Awake()
    {
        movableEntity = GetComponent<MovableEntity>();
    }

    private void BeAware()
    {
        RaycastHit2D[] contacts = Physics2D.CircleCastAll(transform.position, radius, Vector2.one, Mathf.Infinity, contactLayer);
        foreach(RaycastHit2D contact in contacts)
        {
            if(contact.collider.gameObject != gameObject)
            {
                var force = ((Vector2)transform.position - contact.point).normalized * strength;
                movableEntity.ApplyCrudeForce(force);
            }
        }

    }
    
    void FixedUpdate()
    {
        BeAware();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
