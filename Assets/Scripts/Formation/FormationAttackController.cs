using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationAttackController : MonoBehaviour, IManager
{
    private float cooldown;
    private AttackPatternTemplate attackPattern;
    private List<AttackController> attackers = new List<AttackController>();

    void Start()
    {
        attackPattern = GetComponent<AttackPatternTemplate>();

        attackPattern.OnSequenceEnd += StartCooldown;
        cooldown = attackPattern.GetCooldown();

        StartCoroutine(OnCooldown());
    }


    public void AddAttacker(AttackController attacker)
    {
        attackers.Add(attacker);
    }
    private void StartCooldown(object sender, EventArgs e)
    {
        StartCoroutine(OnCooldown());
    }

    private IEnumerator OnCooldown()
    {
        Debug.Log("cooldown started");
        float _cooldown = cooldown;
        while (_cooldown >= 0)
        {
            yield return new WaitForSecondsRealtime(.1f);
            _cooldown -= .1f;
        }
        Debug.Log("cooldown finished");
        StartCoroutine(attackPattern.Sequence(attackers));
        StopCoroutine(OnCooldown());
    }

    public void DestroyManager()
    {
        attackPattern.OnSequenceEnd -= StartCooldown;
    }
}
