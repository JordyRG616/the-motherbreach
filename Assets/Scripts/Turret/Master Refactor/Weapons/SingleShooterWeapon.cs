using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShooterWeapon : Weapon
{
    protected override void SetInitialEffect()
    {
        totalEffect += ApplyDamage;
    }
}
