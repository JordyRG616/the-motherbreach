using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : MonoBehaviour
{
    private FormationMovementController enemy;
    private BossController boss;

    void Start()
    {
        enemy = GetComponentInChildren<FormationMovementController>();
        boss = GetComponent<BossController>();
        if(enemy != null) StartCoroutine(SlowDownEnemy());
        else StartCoroutine(SlowDownBoss());
    }
    
    private IEnumerator SlowDownEnemy()
    {
        enemy.AddSpeedModifier(-0.33f);

        yield return new WaitForSeconds(2f);

        enemy.AddSpeedModifier(0);

        Terminate();
    }

    private IEnumerator SlowDownBoss()
    {
        boss.SetSpeedModifier(-0.33f);

        yield return new WaitForSeconds(2f);

        boss.SetSpeedModifier(0);

        Terminate();
    }

    private void Terminate()
    {
        Destroy(this);
    }
}
