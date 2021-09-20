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


    protected abstract void Move();
}
