using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartAnimation : UIAnimations
{
    private Animator animator;
    [SerializeField] private string firstTrigger = "IN";
    [SerializeField] private string secondTrigger = "OUT";

    public override bool Done { get; protected set; }

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override IEnumerator Forward()
    {
        if(PlaySFX) AudioManager.Main.RequestGUIFX(OnStartSFX, out var index);

        animator.SetTrigger(firstTrigger);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForEndOfFrame();

        Done = true;
    }

    public override IEnumerator Reverse()
    {
        if(PlayReverseSFX) AudioManager.Main.RequestGUIFX(OnReverseSFX, out var index);

        animator.SetTrigger(secondTrigger);
        yield return new WaitForEndOfFrame();

    }

}
