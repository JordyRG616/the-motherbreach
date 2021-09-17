using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryController : ActionController
{
    public override void Awake()
    {
        base.Awake();

        StartCoroutine(GetTarget());
    }

    public override void Activate()
    {
        ActionEffect firstGun = shooters[0];
        ActionEffect secondGun = shooters[1];

        firstGun.Shoot();
        secondGun.Invoke("Shoot", secondGun.GetData().fireRate / 2);
    }
}
