using System.Collections.Generic;
using UnityEngine;

public class HunterTargetSystem : TargetSystem
{
    
    private Collider2D fieldOfView;
    private List<Collider2D> contacts = new List<Collider2D>();
    [SerializeField] private float maxAngle;
    [SerializeField] private bool rotate = true;

    private void Start()
    {
        fieldOfView = GetComponent<Collider2D>();
    }

    private void Update()
    {
        GetTarget();
        if (MaxReach())
        {
            target = null;
        }


        if (target == null)
        {
            ResetRotation();
            return;
        }
        HuntTarget();
    }

    private void ResetRotation()
    {
        if (transform.localEulerAngles.z == 0) return;

        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void GetTarget()
    {
        if (target != null) return;
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
        if (!rotate) return;
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (MaxReach()) return;

        this.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private bool MaxReach()
    {
        if (transform.localEulerAngles.z >= maxAngle && transform.localEulerAngles.z <= 360 - maxAngle) return true;
        return false;
    }

    public void SetTurnSpeed(float value)
    {
    }
}
