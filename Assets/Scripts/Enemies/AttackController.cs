using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private ActionEffect action;
    public int cooldown;

    internal void Attack()
    {
        action.RotateShoots();
        action.Burst();
    }
    
}
