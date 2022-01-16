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
        animator.SetTrigger(firstTrigger);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForEndOfFrame();

        Done = true;
    }

    public override IEnumerator Reverse()
    {
        animator.SetTrigger(secondTrigger);
        yield return new WaitForEndOfFrame();

    }

}
