using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    public int index;
    public string slotID;
    public GameObject occupyingTurret{get; private set;}
    private bool occupied = false;

    public bool IsOcuppied()
    {
        return occupied;
    }

    public void BuildTurret(GameObject turret)
    {
        occupyingTurret = turret;
        if(occupyingTurret.TryGetComponent<TrackingDevice>(out var device))
        {   
            device.StopTracking();
        }
        occupyingTurret.transform.SetParent(transform);
        occupyingTurret.transform.rotation = transform.rotation;
        occupyingTurret.transform.localPosition = Vector2.zero;
        occupyingTurret.GetComponent<TurretManager>().ReceiveInitialRotation(transform.eulerAngles.z);
        occupyingTurret.GetComponent<TurretManager>().slotId = slotID;

        foreach(TurretVFXManager vfx in occupyingTurret.GetComponentsInChildren<VFXManager>())
        {   
            vfx.DisableSelected();
            vfx.InitiateBuild();
        }

        occupied = true;
    }

    public void Clear()
    {
        occupied = false;
        occupyingTurret = null;
    }

}
