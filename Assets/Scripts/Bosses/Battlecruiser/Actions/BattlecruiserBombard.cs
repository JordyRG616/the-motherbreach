using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiserBombard : BossAction
{
    [SerializeField] private List<ActionEffect> rightCannons;
    [SerializeField] private List<ActionEffect> leftCannons;
    [SerializeField] [FMODUnity.EventRef] private string openSFX;
    [SerializeField] [FMODUnity.EventRef] private string closeSFX;

    public override void Start()
    {
        base.Start();

        rightCannons.ForEach(x => x.Initiate());
        rightCannons.ForEach(x => x.ReceiveTarget(ship.gameObject));
        leftCannons.ForEach(x => x.Initiate());
        leftCannons.ForEach(x => x.ReceiveTarget(ship.gameObject));
    }

    public override void Action()
    {

    }

    public override void DoActionMove()
    {

    }

    public override void EndAction()
    {
        rightCannons.ForEach(x => x.StopShooting());
        leftCannons.ForEach(x => x.StopShooting());
        AudioManager.Main.RequestSFX(closeSFX);
        
    }

    public override void StartAction()
    {
        var sign = GetComponent<BattlecruiserIdle>().sign;

        AudioManager.Main.RequestSFX(openSFX);

        if(sign == 1) controller.ActivateAnimation("Right");
        if(sign == -1) controller.ActivateAnimation("Left");
    }

    public void SHOOTRIGHTCANNONS()
    {
        rightCannons.ForEach(x => x.Shoot());
    }

    public void SHOOTLEFTCANNONS()
    {
        leftCannons.ForEach(x => x.Shoot());
    }
}
