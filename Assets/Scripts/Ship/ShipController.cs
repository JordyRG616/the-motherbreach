 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private MovableEntity movableEntity;
    private float angularDrag;

    void OnEnable()
    {
        InputManager.Main.OnMovementPressed += MoveShip;
        InputManager.Main.OnRotationPressed += RotateShip;
    }


    void Awake()
    {
        movableEntity = GetComponent<MovableEntity>();
    }
    
    private void MoveShip(object sender, MovementEventArgs e)
    {
        movableEntity.ApplyForce(e.direction);
    }

    private void RotateShip(object sender, RotationEventArgs e)
    {
        transform.Rotate(0, 0, e.direction * rotationSpeed, Space.Self);
    }

    private void DestroyController()
    {
        InputManager.Main.OnMovementPressed -= MoveShip;
        InputManager.Main.OnRotationPressed -= RotateShip;

        Destroy(this);
    }

}
