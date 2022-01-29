using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : MonoBehaviour
{

    public string stateName;
    protected Rigidbody2D body;
    protected BossAttackController actionController;
    protected Transform ship;
    public string animatorTrigger;


    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        actionController = GetComponent<BossAttackController>();
        ship = ShipManager.Main.transform;
    }

    public abstract void EnterState();
    public abstract void ExitState();

    public abstract void Action();
}
