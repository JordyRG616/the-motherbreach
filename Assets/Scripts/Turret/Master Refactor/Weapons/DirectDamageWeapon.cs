using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectDamageWeapon : Weapon
{
    protected override void SetInitialEffect()
    {
        totalEffect += ApplyDamage;
    }
}
