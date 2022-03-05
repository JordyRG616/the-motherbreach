using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : StatusEffect
{
    private FormationMovementController enemy;
    private BossController boss;
    private float modifier;
    private Rigidbody2D body;

    public override Keyword Status => Keyword.Slug;

    protected override void ExtraInitialize(params float[] parameters)
    {
        // enemy = target.GetComponentInChildren<FormationMovementController>();
        // if(enemy == null) boss = target.GetComponent<BossController>();
        body = target.GetComponent<Rigidbody2D>();
        modifier = parameters[0];
    }

    protected override void ApplyEffect()
    {
        
    }

    protected override void RemoveEffect()
    {
        
    }

    void Update()
    {
        if(body == null) return;
        target.GetComponent<Rigidbody2D>().velocity *= (1 - modifier);
    }
}
