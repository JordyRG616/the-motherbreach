using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTargetSystem : TargetSystem
{
    public Transform additionalTarget { get; private set; }
    private Collider2D fieldOfView;
    private List<Collider2D> contacts = new List<Collider2D>();
    private float refSpeed;
    [SerializeField] private float trackSpeed;
    [SerializeField] private float maxAngle;
    [SerializeField] private Transform bottomWeapon;
    [SerializeField] private Transform topWeapon;

    private void Start()
    {
        fieldOfView = GetComponent<Collider2D>();
    }

    private void Update()
    {
        GetTarget();
        if (target != null && !MaxBottomReach())
        {
            HuntTarget_Bottom();
        }

        if (additionalTarget != null && !MaxTopReach())
        {
            HuntTarget_Top();
        }
    }

    //private void MenageWeapon(Transform _weapon, Transform weaponTarget)
    //{
    //    GetTarget(weaponTarget);
    //    if (weaponTarget == null)
    //    {
    //        ResetRotation(weaponTarget);
    //        return;
    //    }

    //    if (MaxReach(_weapon)) return;

    //    HuntTarget(_weapon);
    //}

    private void ResetBottomRotation()
    {
        if (bottomWeapon.localEulerAngles.z == 0) return;
        float _angle = Mathf.SmoothDampAngle(bottomWeapon.localEulerAngles.z, 0, ref refSpeed, trackSpeed / 10);

        bottomWeapon.localRotation = Quaternion.Euler(0, 0, _angle);
    }

    private void ResetTopRotation()
    {
        if (topWeapon.localEulerAngles.z == 0) return;
        float _angle = Mathf.SmoothDampAngle(topWeapon.localEulerAngles.z, 0, ref refSpeed, trackSpeed / 10);

        topWeapon.localRotation = Quaternion.Euler(0, 0, _angle);
    }

    private void GetTarget()
    {
        var count = fieldOfView.GetContacts(contacts);
        if (count == 0)
        {
            target = null;
            return;
        }

        if(target == null)
        { 
            var rdm = Random.Range(0, count);
            target = contacts[rdm].transform;

            //contacts.RemoveAt(rdm);
        }

        if (count - 1 == 0)
        {
            additionalTarget = null;
            return;
        }

        if (additionalTarget == null)
        {
            var rdm = Random.Range(0, count);
            additionalTarget = contacts[rdm].transform;
        }
    }

    private void HuntTarget_Bottom()
    {
        Vector3 direction = target.transform.position - bottomWeapon.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float _angle = Mathf.SmoothDampAngle(bottomWeapon.eulerAngles.z, angle - 90f, ref refSpeed, trackSpeed / 10);

        bottomWeapon.rotation = Quaternion.Euler(0, 0, _angle);
    }

    private void HuntTarget_Top()
    {
        Vector3 direction = additionalTarget.transform.position - topWeapon.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float _angle = Mathf.SmoothDampAngle(topWeapon.eulerAngles.z, angle - 90f, ref refSpeed, trackSpeed / 10);

        topWeapon.rotation = Quaternion.Euler(0, 0, _angle);
    }

    private bool MaxBottomReach()
    {
        if (bottomWeapon.localEulerAngles.z > maxAngle && bottomWeapon.localEulerAngles.z < 360 - maxAngle) return true;
        return false;
    }

    private bool MaxTopReach()
    {
        if (topWeapon.localEulerAngles.z > maxAngle && topWeapon.localEulerAngles.z < 360 - maxAngle) return true;
        return false;
    }

    public void SetTurnSpeed(float value)
    {
        trackSpeed = value;
    }
}
