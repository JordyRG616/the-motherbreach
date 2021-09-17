using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float movementSpeed, rotationSpeed, dragFactor, dragSpeed;
    private Rigidbody2D body;
    private Vector3 drag;
    private bool playing;

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
        transform.position += drag + (Vector3)e.direction * .01f * movementSpeed;
        // Debug.Log(e.direction.x / dragFactor);
        drag += (Vector3)e.direction / dragFactor;
    }

    private void RotateShip(object sender, RotationEventArgs e)
    {
        body.MoveRotation(body.rotation + e.direction * rotationSpeed);
    }

    private void ApplyInertia(object sender, EventArgs e)
    {
        if(!playing)
        {
            StartCoroutine(Draft());
        }
    }

    private IEnumerator Draft()
    {
        if(drag.magnitude > 0)
        {
            Debug.Log("playing");

            playing = true;

            float dragRemaining = 1.1f;
            
            while(dragRemaining > 1)
            {

                dragRemaining = Mathf.Max(drag.magnitude * dragFactor, 1);

                transform.position += drag * dragSpeed * movementSpeed;
                drag -= drag * .01f;
                Debug.Log(dragRemaining);
                yield return new WaitForSecondsRealtime(.01f);
            }

            drag = Vector3.zero;

            playing = false;

            Debug.Log("finished");

            StopCoroutine(Draft());
        }
    }

    private void DestroyController()
    {
        InputManager.Main.OnMovementPressed -= MoveShip;
        InputManager.Main.OnRotationPressed -= RotateShip;

        Destroy(this);
    }
}
