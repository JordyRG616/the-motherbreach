using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementPatternTemplate : MonoBehaviour
{

    [SerializeField] protected float speed;
    protected Transform ship;

    protected virtual void Awake()
    {
        ship = ShipManager.Main.transform;
    }


    public virtual void Move(out IEnumerator activeCourotine)
    {
        StopAllCoroutines();
        activeCourotine = DoMove();
        StartCoroutine(activeCourotine);
    }

    protected abstract IEnumerator DoMove();

    public void Stop()
    {
        StopAllCoroutines();
    }
}
