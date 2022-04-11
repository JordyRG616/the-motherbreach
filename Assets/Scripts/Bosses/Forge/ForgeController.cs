using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeController : BossController
{
    [SerializeField] private int maxChildCount;
    public List<FormationManager> Children {get; private set;} = new List<FormationManager>();
 
    protected override void ThirdPhaseUpgrade()
    {
       
    }

    protected override void SecondPhaseUpgrade()
    {
       
    }

    public bool HasMaxCapacity()
    {
        return Children.Count >= maxChildCount;
    }
}
