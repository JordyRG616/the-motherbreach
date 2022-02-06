 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float rotationSpeed;
    [SerializeField] [FMODUnity.EventRef] private string onMovementSFX;
    [SerializeField] private List<ParticleSystem> burners;
    private Rigidbody2D body;
    private FMOD.Studio.EventInstance _instance = new FMOD.Studio.EventInstance();
    private float inertia;


    void OnEnable()
    {
        InputManager.Main.OnMovementPressed += MoveShip;
        InputManager.Main.OnRotationPressed += RotateShip;
    }


    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        inertia = body.inertia;
    }
    
    private void MoveShip(object sender, MovementEventArgs e)
    {
        if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        var angle = Mathf.Atan2(-e.direction.y, -e.direction.x) * Mathf.Rad2Deg;
        foreach(ParticleSystem burner in burners)
        {
            burner.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            burner.Play();
        }
        body.velocity = Vector2.zero;
        body.AddForce(e.direction * force, ForceMode2D.Impulse);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && ClickEnabled())
        {
            AudioManager.Main.RequestSFX(onMovementSFX, out _instance);
        }

        if(Input.GetKeyUp(KeyCode.Mouse0) && GameManager.Main.gameState == GameState.OnWave && !GameManager.Main.onPause)
        {
            StopFX();
        }
    }

    public void StopFX()
    {
        AudioManager.Main.StopSFX(_instance);
        foreach (ParticleSystem burner in burners)
        {
            burner.Stop();
        }
    }

    public bool ClickEnabled()
    {
        return GameManager.Main.gameState == GameState.OnWave && !GameManager.Main.onPause && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private void RotateShip(object sender, RotationEventArgs e)
    {
        // transform.Rotate(0, 0, e.direction * rotationSpeed, Space.Self);
        body.centerOfMass = Vector2.zero;
        body.inertia = inertia;
        body.AddTorque(e.direction * rotationSpeed / 100, ForceMode2D.Impulse);
    }

    private void DestroyController()
    {
        InputManager.Main.OnMovementPressed -= MoveShip;
        InputManager.Main.OnRotationPressed -= RotateShip;

        Destroy(this);
    }

}
