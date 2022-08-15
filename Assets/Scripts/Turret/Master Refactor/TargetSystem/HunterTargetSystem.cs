using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterTargetSystem : TargetSystem
{
    
    private Collider2D fieldOfView;
    private List<Collider2D> contacts = new List<Collider2D>();
    private float refSpeed;
    [SerializeField] private float trackSpeed;
    [SerializeField] private float maxAngle;

    private void Start()
    {
        fieldOfView = GetComponent<Collider2D>();
    }

    private void Update()
    {
        GetTarget();
        if(target == null)
        {
            ResetRotation();
            return;
        }

        if (MaxReach()) return;

        HuntTarget();
    }

    private void ResetRotation()
    {
        if (transform.localEulerAngles.z == 0) return;
        float _angle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, 0, ref refSpeed, trackSpeed / 10);

        this.transform.localRotation = Quaternion.Euler(0, 0, _angle);
    }

    private void GetTarget()
    {
        var count = fieldOfView.GetContacts(contacts);
        if (count == 0)
        {
            target = null;
            return;
        }



        var rdm = Random.Range(0, count);
        target = contacts[rdm].transform;
    }

    private void HuntTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, angle - 90f, ref refSpeed, trackSpeed / 10);

        this.transform.rotation = Quaternion.Euler(0, 0, _angle);
    }

    private bool MaxReach()
    {
        if (transform.localEulerAngles.z > maxAngle && transform.localEulerAngles.z < 360 - maxAngle) return true;
        return false;
    }

    public void SetTurnSpeed(float value)
    {
        trackSpeed = value;
    }
}
