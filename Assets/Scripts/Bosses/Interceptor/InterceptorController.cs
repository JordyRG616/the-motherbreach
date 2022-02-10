using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorController : BossController
{
    [SerializeField] private float range;
    [SerializeField] private float disengageDistance;
    [SerializeField] private float disengageTime;
    [SerializeField] private int attacksToSpecial;
    private int attacksMade;
    [SerializeField] private float offenseDuration;
    private float timer;
    private float runningTimer;
    private bool countTimer;
    private bool specialOn;
    private bool running;
    private bool secondPhaseActivated;
    private bool thirdPhaseActivated;
    [Header("Second Phase Upgrades")]
    [SerializeField] private float emissionModifier;
    [SerializeField] private float orbitSpeedModifier;
    [Header("Third Phase Upgrade")]
    [SerializeField] private float specialDurationModifier;


    protected override void ManageStates(int order)
    {
        if(running)
        {
            if(runningTimer < disengageTime) return;
            runningTimer = 0;
            timer = 0;
            countTimer = false;
            running = false;
            return;
        } 
        var distance = Vector2.Distance(ship.position, transform.position);
        if(distance <= disengageDistance && !specialOn)
        {
            Disengage();
            return;
        }
        switch (order)
        {
            case 0:
                FirstPhase(distance);
            break;
            case 1:
                SecondPhase(distance);
            break;
            case 2:
                SecondPhase(distance);
            break;
        }
    }

    private void Disengage()
    {
        running = true;
        countTimer = false;
        timer = 0;
        var disengageState = GetComponent<Interceptor_Disengage>();
        ChangeStates(disengageState);
    }

    protected override void PhaseUpgrade(int order)
    {
        switch(order)
        {
            case 0:
            break;
            case 1:
                SecondPhaseUpgrade();
            break;
            case 2:
                ThirdPhaseUpgrade();
            break;
        }
    }

    private void FirstPhase(float distance)
    {
        if (distance > range)
        {
            var moveState = activePhase.states.Find(t => t.stateName == "Move");
            ChangeStates(moveState);
        }
        if (distance <= range - 5)
        {
            var attackState = activePhase.states.Find(t => t.stateName == "Attack");
            ChangeStates(attackState);
        }
    }

    private void SecondPhaseUpgrade()
    {
        if(secondPhaseActivated) return;
        Disengage();
        var weaponsToUpgrade = GetComponent<BossAttackController>().RetrieveWeapons(WeaponClass.Artillery);
        foreach(ActionEffect weapon in weaponsToUpgrade)
        {
            var emission = weapon.GetShooterSystem().emission;
            emission.rateOverTimeMultiplier += emissionModifier;
        }
        GetComponent<Interceptor_AttackState>().speedMultiplier = orbitSpeedModifier;
        GetComponent<Interceptor_Disengage>().offensiveRetreat = true;
        GetComponent<Interceptor_Disengage>().ignoreAnimation = true;
        disengageTime /= 2;
        secondPhaseActivated = true;
    }

    private void SecondPhase(float distance)
    {
        if (timer >= offenseDuration)
        {
            specialOn = false;
            Disengage();
            return;
        }
        if (distance > range && countTimer == false)
        {
            countTimer = false;
            var moveState = activePhase.states.Find(t => t.stateName == "Move");
            ChangeStates(moveState);
            return;
        }
        if (distance <= range - 5 && countTimer == false)
        {
            timer = 0;
            countTimer = true;

            if(attacksMade == attacksToSpecial)
            {
                var specialState = activePhase.states.Find(t => t.stateName == "Special");
                ChangeStates(specialState);
                specialOn = true;
                attacksMade = 0;
            }
            else 
            {
                var attackState = activePhase.states.Find(t => t.stateName == "Attack");
                ChangeStates(attackState);
                attacksMade++;
            }
        }
    }

    private void ThirdPhaseUpgrade()
    {
        if(thirdPhaseActivated) return;
        Disengage();
        offenseDuration *= specialDurationModifier; 
        thirdPhaseActivated = true;
    }


    protected override void Update()
    {
        if(countTimer) 
        {
            timer += Time.deltaTime;
        }
        if(running)
        {
            runningTimer += Time.deltaTime;
        }
        base.Update();
    }

    
}
