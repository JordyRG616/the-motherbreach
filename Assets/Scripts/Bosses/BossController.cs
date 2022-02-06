using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : MonoBehaviour, IManager
{
    [SerializeField] protected List<BossPhase> phases;
    protected BossHealthController healthController;
    protected BossPhase activePhase;
    protected BossState activeState;
   protected Transform ship;

    protected Animator animator;

    protected delegate void StateAction();
    protected StateAction action;

    protected virtual void Awake()
    {
        healthController = GetComponent<BossHealthController>();
        healthController.Initiate();
        animator = GetComponent<Animator>();
        ship = ShipManager.Main.transform;
    }

    public void VerifyPhase()
    {
        var percentage = healthController.GetHealthPercentage();
        activePhase = phases[0];
        foreach(BossPhase phase in phases)
        {
            if(percentage > phase.threshold)
            {
                continue;
            } 
            else if(activePhase.order <= phase.order)
            {
                activePhase = phase;
                PhaseUpgrade(activePhase.order);
            }
        }
    }

    protected abstract void PhaseUpgrade(int phaseOrder);

    protected abstract void ManageStates(int phaseOrder);

    protected void ChangeStates(BossState newState)
    {
        if(activeState == newState) return;
        // Debug.Log(newState);
        if(activeState != null) 
        {
            activeState.ExitState();
            animator.SetBool(activeState.animatorTrigger, false);
        }
        activeState = newState;
        activeState.EnterState();
        if(!activeState.ignoreAnimation) animator.SetBool(activeState.animatorTrigger, true);
        action = null;
        action = activeState.Action;
    }

    protected virtual void Update()
    {
        ManageStates(activePhase.order);
        action?.Invoke();
    }

    public List<float> ReturnThresholds()
    {
        var container = new List<float>();

        foreach(BossPhase phase in phases)
        {
            container.Add(phase.threshold);
        }

        return container;
    }

    public void DestroyManager()
    {
        WaveManager.Main.RemoveBoss(this);
    }

    public void SetSpeedModifier(float value)
    {
        foreach(BossState state in GetComponents<BossState>())
        {
            state.speedMultiplier = value;
        }
    }

    public void Sleep()
    {
        action = null;
    }
}

[System.Serializable]
public struct BossPhase
{
    public int order;
    public List<BossState> states;
    public float threshold;
}