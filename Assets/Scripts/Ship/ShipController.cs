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
        transform.position += linearDrag + (Vector3)e.direction * .01f * movementSpeed;
        // Debug.Log(e.direction.x / dragFactor);
        linearDrag += (Vector3)e.direction / dragFactor;
    }

    private void RotateShip(object sender, RotationEventArgs e)
    {
        transform.Rotate(0, 0, e.direction * rotationSpeed, Space.Self);
        angularDrag += e.direction / dragFactor;
    }

    private void ApplyInertia(object sender, EventArgs e)
    {
        if(!linearDrafting)
        {
            StartCoroutine(MovementDraft());
        }
        if(!angularDrafting)
        {
            StartCoroutine(RotationDraft());
        }
    }

    private IEnumerator MovementDraft()
    {
        if(linearDrag.magnitude > 0)
        {
            linearDrafting = true;

            float dragRemaining = 1.11f;
            
            while(dragRemaining > 1)
            {

                if (dragRemaining <= 1.1) dragRemaining = Mathf.Max(linearDrag.magnitude * dragFactor, 1);
                if (dragRemaining > 1.1) dragRemaining = 1.1f;

                transform.position += linearDrag * dragSpeed * movementSpeed;
                linearDrag -= linearDrag * .01f;
                yield return new WaitForSecondsRealtime(.01f);
            }

            linearDrag = Vector3.zero;

            linearDrafting = false;

            StopCoroutine(MovementDraft());
        }
    }

    private IEnumerator RotationDraft()
    {        
        angularDrafting = true;

        float dragRemaining = 1.1f;
        
        while(dragRemaining > 1)
        {

            if (dragRemaining <= 2) dragRemaining = Mathf.Max(Mathf.Sqrt((angularDrag * dragFactor) * (angularDrag * dragFactor)), 1);
            if (dragRemaining > 2) dragRemaining = 2;


            transform.Rotate(0, 0, angularDrag * dragSpeed * rotationSpeed * angularDraftAdjust, Space.Self);
            angularDrag -= angularDrag * .01f;
            yield return new WaitForSecondsRealtime(.01f);
        }

        //angularDrag = 0;

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
