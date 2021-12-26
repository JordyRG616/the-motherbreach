using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    public GameObject occupyingTurret{get; private set;}
    private bool occupied = false;

    public bool IsOcuppied()
    {
        return occupied;
    }

    public void BuildTurret(GameObject turret)
    {
        //occupyingTurret = Instantiate(turret, Vector3.zero, transform.rotation, transform);
        // turret.transform.position = transform.position;
        occupyingTurret = turret;
        occupyingTurret.GetComponent<TrackingDevice>().StopTracking();
        occupyingTurret.transform.SetParent(transform);
        occupyingTurret.transform.rotation = transform.rotation;
        occupyingTurret.transform.localPosition = Vector2.zero;

        occupyingTurret.GetComponentInChildren<TurretVFXManager>().DisableSelected();

        occupied = true;
    }

    public void Clear()
    {
        occupied = false;
        occupyingTurret = null;
    }

}
