using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartAnimation : UIAnimations
{
    private Animator animator;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override IEnumerator Forward()
    {
        animator.SetTrigger("IN");
        yield return new WaitForEndOfFrame();
    }

    protected override IEnumerator Reverse()
    {
        animator.SetTrigger("OUT");
        yield return new WaitForEndOfFrame();

    }

}
