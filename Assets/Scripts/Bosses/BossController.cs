using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : MonoBehaviour, IManager
{
    [SerializeField] protected List<BossPhase> phases;
    [SerializeField] protected BossIdle idle;
    [SerializeField] protected float intervalToCheck;

    [SerializeField] protected ParticleSystem phaseFeedbackVFX;
    [SerializeField] [FMODUnity.EventRef] protected string phaseFeedbackSFX;
    [SerializeField] protected ParticleSystem secondPhaseTrail;

    [SerializeField] protected string bossMusic;
    [SerializeField] protected string musicParameterName;
    protected FMOD.Studio.EventInstance musicInstance;

    protected BossHealthController healthController;
    protected BossPhase activePhase;
    protected BossAction currentAction;
    protected Transform ship;
    protected float chanceToAct = 0;
    protected WaitForSeconds waitTime;
    protected bool onAction;
    public string currentTrigger {get; protected set;}
    protected int upgraded;

    protected Animator animator;

    public delegate void BossMovement();
    public BossMovement movement;
    public delegate void ActiveAction();
    public ActiveAction bossAction;


    protected virtual void Awake()
    {
        AudioManager.Main.RequestBossMusic(bossMusic, out musicInstance);

        healthController = GetComponent<BossHealthController>();
        healthController.Initiate();
        animator = GetComponent<Animator>();
        ship = ShipManager.Main.transform;

        waitTime = new WaitForSeconds(intervalToCheck);

        movement = idle.IdleMove;

        StartCoroutine(ManageActions());

        phases.ForEach(x => x.Initiate());
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

    protected virtual void PhaseUpgrade(int phaseOrder)
    {
        switch(phaseOrder)
        {
            case 1:
                if(upgraded >= 1) return;
                SecondPhaseUpgrade();
                secondPhaseTrail.Play();
                musicInstance.setParameterByName(musicParameterName, 1);
                StartCoroutine(PhaseFeedbackEffect());
                FindObjectOfType<BossHealthBar>().ActivatePhaseMarker(1);
                upgraded++;
            break;
            case 2:
                if(upgraded >= 2) return;
                ThirdPhaseUpgrade();
                musicInstance.setParameterByName(musicParameterName, 2);
                StartCoroutine(PhaseFeedbackEffect());
                FindObjectOfType<BossHealthBar>().ActivatePhaseMarker(2);
                upgraded++;
            break;
        }
    }

    protected virtual IEnumerator PhaseFeedbackEffect()
    {
        phaseFeedbackVFX.Play();
        AudioManager.Main.RequestSFX(phaseFeedbackSFX);
        Time.timeScale = 0.35f;
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 1f;

        // StartCoroutine(Act());
    }

    protected abstract void SecondPhaseUpgrade();

    protected abstract void ThirdPhaseUpgrade();

    protected virtual IEnumerator ManageActions()
    {
        while(true)
        {
            var rdm = Random.Range(0, 1f);

            if(rdm < chanceToAct && !onAction)
            {
                yield return StartCoroutine(Act());
            }
            else chanceToAct += 0.1f; 

            yield return waitTime; 
        }
    }

    protected virtual IEnumerator Act()
    {
        currentAction?.EndAction();

        onAction = true;

        var actions = GetCombo().actions;

        foreach(BossAction action in actions)
        {
            currentAction = action;
            currentAction.StartAction();
            bossAction = currentAction.Action;
            if(currentAction.hasMove) movement = currentAction.DoActionMove;

            yield return new WaitForSeconds(currentAction.actionDuration);

            currentAction?.EndAction();
            ClearAction();
        }

        ActivateAnimation("Move");

        chanceToAct = 0;
    }
    
    void FixedUpdate()
    {       
        bossAction?.Invoke();
        movement?.Invoke();
    }

    private ActionCombo GetCombo()
    {
        var rdm = Random.Range(0, 1f);
        var windows = activePhase.comboActivationWindows;

        foreach(Vector2 window in windows.Keys)
        {
            if(rdm >= window.x && rdm < window.y) return windows[window];
        }

        return windows.ElementAt(0).Value;

        // var container = activePhase.combos[0];
        
        // foreach(ActionCombo combo in activePhase.combos)
        // {
        //     if(rdm > combo.chance) continue;
        //     else if(combo.chance <= container.chance) container = combo;
        // }

        // return container;
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

    public virtual void DestroyManager()
    {
        WaveManager.Main.RemoveBoss(this);
    }

    public void EndMusicInstance()
    {
        musicInstance.setParameterByName(musicParameterName, 3);
    }

    public void Sleep()
    {

    }

    public void ActivateAnimation(string trigger)
    {
        animator.SetBool(currentTrigger, false);
        animator.SetBool(trigger, true);
        currentTrigger = trigger;
    }

    public void TRIGGERDELAYEDACTION()
    {
        currentAction.InitiateDelayedAttack();
    }

    public void ClearAction()
    {
        currentAction = null;
        bossAction = null;
        movement = idle.IdleMove;
        onAction = false;
    }
}

[System.Serializable]
public class BossPhase
{
    public int order;
    public List<ActionCombo> combos;
    public float threshold;
    public Dictionary<Vector2, ActionCombo> comboActivationWindows;

    public void Initiate()
    {
        comboActivationWindows = new Dictionary<Vector2, ActionCombo>();
        var window = Vector2.zero;

        foreach(ActionCombo combo in combos)
        {
            window.x = window.y;
            window.y += combo.chance;
            comboActivationWindows.Add(window, combo);
        }
    }
}