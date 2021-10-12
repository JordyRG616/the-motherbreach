using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffectTemplate : MonoBehaviour
{
    [SerializeField] protected BaseEffectTrigger trigger;
    protected ActionController associatedController;

    public abstract void ApplyEffect();

    
    protected void UpdateControllerStats()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            shooter.SetActionData();
        }
    }

    public void ReceiveWeapon(ActionController weapon)
    {
        associatedController = weapon;
    }

    public BaseEffectTrigger GetTrigger()
    {
        return trigger;
    }

}
