using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadComponent : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private int angleVariation;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private float maxDistance, minDistance;
    private float _angle;
    private float rotatedAngle
    {
        get => _angle;

        set
        {
            if(value > 1) _angle = 1;
            else if(value < -1) _angle = -1;
            else _angle = value;
        }
    }
    private float ogAngle;
    private float timer;
    private InputManager inputManager;


    void Awake()
    {
        inputManager = InputManager.Main;
        inputManager.OnRotationPressed += HandleRotation;
        inputManager.OnInertia += ResetRotation;
        inputManager.OnMovementPressed += HandleExpansion;
        ogAngle = transform.eulerAngles.z;
    }

    private void HandleExpansion(object sender, MovementEventArgs e)
    {
        var _d = e.direction;
        var distance = Vector2.Distance(transform.localPosition, Vector2.zero);

        if(_d.magnitude == 0 && distance > minDistance)
        {
            transform.localPosition -= direction * 0.1f;
        }

        if(_d.magnitude > 0 && distance < maxDistance)
        {
            transform.localPosition += direction * 0.1f;
        }
        
    }

    private void ResetRotation(object sender, EventArgs e)
    {
        if(Mathf.Abs(rotatedAngle) > 0)
        {
            var sign = Mathf.Sign(0 - rotatedAngle);
            rotatedAngle += 0.01f * sign;
            var newAngle = rotationCurve.Evaluate(rotatedAngle);
            newAngle += ogAngle;
            transform.localRotation = Quaternion.Euler(0, 0, newAngle);
        }
    }

    private void HandleRotation(object sender, RotationEventArgs e)
    {
        var direction = - e.direction;

        rotatedAngle += 0.01f * direction;
        var newAngle = rotationCurve.Evaluate(rotatedAngle);
        newAngle += ogAngle;
        transform.localRotation = Quaternion.Euler(0, 0, newAngle);
        
    }
}
