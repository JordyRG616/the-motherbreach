using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAction : BossAction
{
    [SerializeField] private float range;
    [SerializeField] private ParticleSystem teleportIn;
    [SerializeField] private ParticleSystem teleportOut;

    [Header("Decoy")]
    public bool deployDecoy;
    [SerializeField] private GameObject decoy;

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
        // controller.ActivateAnimation("Teleport");
        animator.SetBool("Teleport", true);
        teleportIn.Play();
        teleporting = true;
    }

    public void FINISHTELEPORT()
    {
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

        if(deployDecoy) DeployDecoy(rdmPos);
    }

    private void DeployDecoy(Vector2 rdmPos)
    {
        rdmPos = (Vector2)ship.position - rdmPos;

        Instantiate(decoy, rdmPos, Quaternion.identity);
    }

    public override void StartAction()
    {
        InitiateTeleport();
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
