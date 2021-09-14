using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float movementSpeed, rotationSpeed;
    private Rigidbody2D body;

    void OnEnable()
    {
        InputManager.Main.OnMovementPressed += MoveShip;
        InputManager.Main.OnRotationPressed += RotateShip;
    }


    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void MoveShip(object sender, MovementEventArgs e)
    {
        body.AddForce(e.direction * movementSpeed, ForceMode2D.Force);
    }

    private void RotateShip(object sender, RotationEventArgs e)
    {
        body.MoveRotation(body.rotation + e.direction * rotationSpeed);
    }

    private void DestroyController()
    {
        InputManager.Main.OnMovementPressed -= MoveShip;
        InputManager.Main.OnRotationPressed -= RotateShip;

        Destroy(this);
    }
}
