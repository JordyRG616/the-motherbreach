using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalBurn : MonoBehaviour
{
    private HitManager hitManager;
    private float damage = 3f;

    void Start()
    {
        hitManager = GetComponent<HitManager>();

        StartCoroutine(Burn());
    }

    private IEnumerator Burn()
    {
        float step = 0;

        while(step <= 3)
        {
            hitManager.HealthInterface.UpdateHealth(-damage);

            step += 0.25f;

            yield return new WaitForSeconds(0.25f);
        }

        Terminate();
    }

    private void Terminate()
    {
        Destroy(this);
    }
}
