using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretActionMediator : MonoBehaviour
{
    public Weapon linkedWeapon;

    public void PassTarget(HitManager target, out float damage)
    {
        linkedWeapon.totalEffect(target);
        damage = linkedWeapon.GetStatValue<Damage>();
        Debug.Log(damage);
    }
}
