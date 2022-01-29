using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : MonoBehaviour
{
    [SerializeField] protected List<BossPhase> phases;
    protected EnemyHealthController healthController;
    protected BossPhase activePhase;
    protected BossState activeState;
   protected Transform ship;

    protected Animator animator;

    protected delegate void StateAction();
    protected StateAction action;

    protected virtual void Awake()
    {
        healthController = GetComponent<EnemyHealthController>();
        healthController.OnDamage += VerifyPhase;
        animator = GetComponent<Animator>();
        ship = ShipManager.Main.transform;

        VerifyPhase(this, System.EventArgs.Empty);
    }

    protected void VerifyPhase(object sender, System.EventArgs e)
    {
        var percentage = healthController.GetHealthPercentage();
        foreach(BossPhase phase in phases)
        {
            if(percentage > phase.threshold) continue;
            else activePhase = phase;
        }
        PhaseUpgrade(activePhase.order);
    }

    protected abstract void PhaseUpgrade(int phaseOrder);

    protected abstract void ManageStates(int phaseOrder);

    protected void ChangeStates(BossState newState)
    {
        if(activeState == newState) return;
        Debug.Log(newState);
        if(activeState != null) 
        {
            activeState.ExitState();
            animator.SetBool(activeState.animatorTrigger, false);
        }
        activeState = newState;
        animator.SetBool(activeState.animatorTrigger, true);
        activeState.EnterState();
        action = activeState.Action;
    }

    protected virtual void Update()
    {
        ManageStates(activePhase.order);
        action?.Invoke();
    }
}

[System.Serializable]
public struct BossPhase
{
    public int order;
    public List<BossState> states;
    public float threshold;
}