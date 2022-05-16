using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAction : BossAction
{
    [SerializeField] private float range;
    [SerializeField] private ParticleSystem teleportIn;
    [SerializeField] private ParticleSystem teleportOut;
    [SerializeField] [FMODUnity.EventRef] private string teleportSFX;
    private FMOD.Studio.EventInstance instance;

    [Header("Decoy")]
    public bool deployDecoy;
    [SerializeField] private GameObject decoy;
    private GameObject activeDecoy;

    private bool teleporting;
    private Animator animator;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public void InitiateTeleport()
    {
        if(teleporting) return;
        animator.SetBool("Teleport", true);
        AudioManager.Main.RequestSFX(teleportSFX, out instance);
        teleportIn.Play();
        teleporting = true;
    }

    public void FINISHTELEPORT()
    {
        var ogPos = transform.position;

        instance.setParameterByName("DelayedTrigger", 1);
        var rdm = Random.Range(0, 2 * Mathf.PI);
        Vector2 rdmPos = new Vector2(Mathf.Cos(rdm), Mathf.Sin(rdm));
        rdmPos *= range;
        var _pos = rdmPos + (Vector2)ship.position;

        GetComponent<TargetableComponent>().Detach();
        transform.position = _pos;
        // controller.ActivateAnimation("Move");
        animator.SetBool("Teleport", false);
        teleportOut.Play();
        teleporting = false;

        if(deployDecoy) DeployDecoy(ogPos);
    }

    private void DeployDecoy(Vector2 rdmPos)
    {
        // rdmPos = (Vector2)ship.position - rdmPos;
        if(activeDecoy != null) return;
        activeDecoy = Instantiate(decoy, rdmPos, Quaternion.identity);
    }

    public override void StartAction()
    {
        InitiateTeleport();
        controller.StopMovementSFX();
    }

    public override void Action()
    {
    }

    public override void EndAction()
    {
    }

    public override void DoActionMove()
    {
        var direction = ship.position - transform.position;
        LookAt(direction);
    }
}
