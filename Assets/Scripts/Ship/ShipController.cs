using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float movementSpeed, rotationSpeed, dragFactor, dragSpeed;
    private Rigidbody2D body;
    private Vector3 linearDrag;
    private float angularDrag;
    private bool linearDrafting, angularDrafting;
    [SerializeField] private float angularDraftAdjust;

    void OnEnable()
    {
        InputManager.Main.OnMovementPressed += MoveShip;
        InputManager.Main.OnRotationPressed += RotateShip;
        InputManager.Main.OnInertia += ApplyInertia;
    }


    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        //StartCoroutine(Draft());
    }
    
    private void MoveShip(object sender, MovementEventArgs e)
    {
        transform.position += (Vector3)e.direction * .01f * movementSpeed;
        linearDrag += (Vector3)e.direction;
    }

    private void RotateShip(object sender, RotationEventArgs e)
    {
        transform.Rotate(0, 0, e.direction * rotationSpeed, Space.Self);
        angularDrag = e.direction;
    }

    private void ApplyInertia(object sender, EventArgs e)
    {
        if(!linearDrafting && linearDrag.magnitude > 0)
        {
            StartCoroutine(MovementDraft());
        }
        if(!angularDrafting && angularDrag > 0)
        {
            StartCoroutine(RotationDraft());
        }
    }

    private IEnumerator MovementDraft()
    {
        if(linearDrag.magnitude > 0)
        {
            linearDrafting = true;

            float step = .1f;
            
            while(step > 0)
            {
                transform.position += linearDrag.normalized * step;
                step -= .001f;
                yield return new WaitForSecondsRealtime(.02f);
            }

            linearDrag = Vector3.zero;

            linearDrafting = false;

            StopCoroutine(MovementDraft());
        }
    }

    private IEnumerator RotationDraft()
    {        
        angularDrafting = true;

        float step = .1f;
        
        while(step > 0)
        {
            transform.Rotate(0, 0, angularDrag * step * 2, Space.Self);
            step -= .001f;
            yield return new WaitForSecondsRealtime(.02f);
        }

        angularDrag = 0;

        angularDrafting = false;

        StopCoroutine(RotationDraft());
        
    }

    private void DestroyController()
    {
        InputManager.Main.OnMovementPressed -= MoveShip;
        InputManager.Main.OnRotationPressed -= RotateShip;

        Destroy(this);
    }

}
