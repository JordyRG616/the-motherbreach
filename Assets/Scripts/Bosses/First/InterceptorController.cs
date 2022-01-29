using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptorController : BossController
{
    [SerializeField] private float range;
    [SerializeField] private float timeToSpecial;
    private float timer;
    private bool countTimer;
    private bool specialOn;

    protected override void ManageStates(int order)
    {
        var distance = Vector2.Distance(ship.position, transform.position);
        switch(order)
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
        throw new NotImplementedException();
    }

    private void SecondPhase(float distance)
    {
        if (timer >= timeToSpecial * 2)
        {
            specialOn = false;
            timer = 0;
            var disengageState = activePhase.states.Find(t => t.stateName == "Disengage");
            ChangeStates(disengageState);
        }
        if (distance > range && !specialOn)
        {
            countTimer = false;
            var moveState = activePhase.states.Find(t => t.stateName == "Move");
            ChangeStates(moveState);
        }
        if (distance <= range - 5 && !specialOn)
        {
            var attackState = activePhase.states.Find(t => t.stateName == "Attack");
            ChangeStates(attackState);

            countTimer = true;
            if (timer >= timeToSpecial && !specialOn)
            {
                var specialState = activePhase.states.Find(t => t.stateName == "Special");
                ChangeStates(specialState);
                specialOn = true;
            }
        }
    }

    private void ThirdPhaseUpgrade()
    {
        throw new NotImplementedException();
    }


    protected override void Update()
    {
        if(countTimer) 
        {
            timer += Time.deltaTime;
        }
        base.Update();
    }

    
}
