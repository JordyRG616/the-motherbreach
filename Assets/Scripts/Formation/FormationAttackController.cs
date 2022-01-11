using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationAttackController : MonoBehaviour, IManager
{
    private float cooldown;
    private AttackPatternTemplate attackPattern;
    private List<EnemyAttackController> attackers = new List<EnemyAttackController>();

    void Start()
    {
        attackPattern = GetComponent<AttackPatternTemplate>();

        attackPattern.OnSequenceEnd += StartCooldown;
        cooldown = attackPattern.GetCooldown();

        StartCoroutine(OnCooldown());
    }


    public void AddAttacker(EnemyAttackController attacker)
    {
        attackers.Add(attacker);
        attacker.OnDeath += RemoveEnemy;
    }

    private void RemoveEnemy(object sender, EnemyEventArgs e)
    {
        e.attackController.OnDeath -= RemoveEnemy;
        attackers.Remove(e.attackController);
    }

    private void StartCooldown(object sender, EventArgs e)
    {
        StartCoroutine(OnCooldown());
    }

    private IEnumerator OnCooldown()
    {
        float _cooldown = cooldown;
        while (_cooldown >= 0)
        {
            yield return new WaitForSeconds(.1f);
            _cooldown -= .1f;
        }
        StartCoroutine(attackPattern.Sequence(attackers));
        StopCoroutine(OnCooldown());
    }

    public void DestroyManager()
    {
        StopAllCoroutines();
        attackPattern.OnSequenceEnd -= StartCooldown;
    }
}
