using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterTargetSystem : MonoBehaviour
{
    
    private Transform target;
    private Collider2D fieldOfView;
    private List<Collider2D> contacts = new List<Collider2D>();
    private float refSpeed;
    [SerializeField] private float trackSpeed;

    private void Update()
    {
        if(target == null)
        {
            GetTarget();
            return;
        }

        HuntTarget();
    }

    private void GetTarget()
    {
        var count = fieldOfView.GetContacts(contacts);
        if (count == 0) return;

        var rdm = Random.Range(0, count);
        target = contacts[rdm].transform;
    }

    private void HuntTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, angle - 90f, ref refSpeed, trackSpeed);

        this.transform.rotation = Quaternion.Euler(0, 0, _angle);
    }
}
