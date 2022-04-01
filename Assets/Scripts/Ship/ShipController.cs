 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipController : MonoBehaviour
{
    [FormerlySerializedAs("force")] [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] [FMODUnity.EventRef] private string onMovementSFX;
    // private List<ParticleSystem> burners = new List<ParticleSystem>();
    private Rigidbody2D body;
    private FMOD.Studio.EventInstance _instance = new FMOD.Studio.EventInstance();
    private float inertia;
    private bool playingSFX;


    void OnEnable()
    {
        InputManager.Main.OnMovementPressed += MoveShip;
        InputManager.Main.OnRotationPressed += RotateShip;
    }


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        inertia = body.inertia;
    }

    // void Start()
    // {
    //     var burnerGO = GameObject.FindGameObjectsWithTag("ShipBurner");

    //     foreach(GameObject go in burnerGO)
    //     {
    //         var burner = go.GetComponent<ParticleSystem>();
    //         burners.Add(burner);
    //     }
    // }
    
    private void MoveShip(object sender, MovementEventArgs e)
    {
        if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || e.direction.magnitude == 0)
        { 
            StopFX();
            return;
        }
        if(!playingSFX) 
        {
            AudioManager.Main.RequestSFX(onMovementSFX, out _instance);
            playingSFX = true;
        }
        var angle = Mathf.Atan2(-e.direction.y, -e.direction.x) * Mathf.Rad2Deg;
        // foreach(ParticleSystem burner in burners)
        // {
        //     burner.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        //     burner.Play();
        // }
        body.velocity = Vector2.zero;
        body.AddForce(e.direction * speed, ForceMode2D.Impulse);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse0) && GameManager.Main.gameState == GameState.OnWave && !GameManager.Main.onPause)
        {
            StopFX();
        }
    }

    public void StopFX()
    {
        AudioManager.Main.StopSFX(_instance);
        playingSFX = false;
        // foreach (ParticleSystem burner in burners)
        // {
        //     burner.Stop();
        // }
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

    public void DestroyController()
    {
        InputManager.Main.OnMovementPressed -= MoveShip;
        InputManager.Main.OnRotationPressed -= RotateShip;

        Destroy(this);
    }

    public void ModifySpeedByPercentage(float percentage)
    {
        speed *= (1 + percentage);
    }

}
