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
        occupyingTurret = turret;
        occupyingTurret.transform.position = Vector3.zero;
        occupyingTurret.transform.rotation = transform.rotation;
        occupyingTurret.transform.SetParent(transform);

        occupyingTurret.GetComponentInChildren<TurretVFXManager>().DisableSelected();
        occupyingTurret.transform.localPosition = Vector3.zero;

        occupied = true;
    }

    public void Clear()
    {
        occupied = false;
        occupyingTurret = null;
    }

}
