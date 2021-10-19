using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    private GameObject occupyingTurret;

    public bool IsOcuppied()
    {
        if(occupyingTurret == null)
        {
            return false;
        }

        return true;
    }

    public void BuildTurret(GameObject turret)
    {
        GameObject occupyingTurret = Instantiate(turret, Vector3.zero, transform.rotation, transform);
        TriggerImeddiateEffect(occupyingTurret);

        occupyingTurret.GetComponentInChildren<TurretVFXManager>().DisableSelected();
        occupyingTurret.transform.localPosition = Vector3.zero;
    }

    private static void TriggerImeddiateEffect(GameObject occupyingTurret)
    {
        BaseEffectTemplate effect = occupyingTurret.GetComponentInChildren<BaseEffectTemplate>();
        effect.ReceiveWeapon(occupyingTurret.GetComponentInChildren<ActionController>());
        if (effect.GetTrigger() == BaseEffectTrigger.Immediate)
        {
            effect.ApplyEffect();
        }

    }

}
