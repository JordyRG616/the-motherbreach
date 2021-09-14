using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementPatternTemplate : MonoBehaviour
{

    [SerializeField] protected float speed;
    [SerializeField] protected Transform baseTransform;


    protected abstract void Move();
}
