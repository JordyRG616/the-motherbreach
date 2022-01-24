using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : MonoBehaviour
{
    private FormationMovementController enemy;

    void Start()
    {
        enemy = GetComponentInChildren<FormationMovementController>();
        StartCoroutine(SlowDown());
    }
    
    private IEnumerator SlowDown()
    {
        enemy.AddSpeedModifier(-0.33f);

        yield return new WaitForSeconds(2f);

        enemy.AddSpeedModifier(0);

        Terminate();
    }

    private void Terminate()
    {
        Destroy(this);
    }
}
