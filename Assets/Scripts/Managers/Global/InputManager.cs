using System;
using System.Collections;
using CraftyUtilities;
using UnityEngine;
//* CLEAN
public class InputManager : MonoBehaviour
{
    #region Singleton
    private static InputManager _instance;
    public static InputManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<InputManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private Settings settings;
    [SerializeField] [FMODUnity.EventRef] private string clearSFX;
    private MovementControlScheme movementScheme = MovementControlScheme.None;
    private RotationControlScheme rotationScheme = RotationControlScheme.None;
    public KeyCode leftKey, rightKey, upKey, downKey; // MOVEMENT RELATED KEYCODES
    public KeyCode rotateRight, rotateLeft; // ROTATION RELATED KEYCODES

    public event EventHandler<MovementEventArgs> OnMovementPressed;
    public event EventHandler<RotationEventArgs> OnRotationPressed;
    public EventHandler OnMovementInertia;
    public event EventHandler OnInertia;
    public event EventHandler OnSelectionClear;
    public event EventHandler OnGamePaused;

    public delegate void ControlScheme();
    public ControlScheme move;


    public void ClearEvents()
    {
        if(OnMovementPressed != null)
        {
            foreach(Delegate d in OnMovementPressed.GetInvocationList())
            {
                OnMovementPressed -= (EventHandler<MovementEventArgs>)d;
            }
        }

        if(OnRotationPressed != null)
        {
            foreach(Delegate d in OnRotationPressed.GetInvocationList())
            {
                OnRotationPressed -= (EventHandler<RotationEventArgs>)d;
            }
        }
    }

    void Start()
    {
        SetKeys();

        move = MouseControlScheme;
    }

    public void SetKeys()
    {
        leftKey = settings.moveLeft;
        rightKey= settings.moveRight;
        upKey = settings.moveUp;
        downKey = settings.moveDown;

        rotateLeft = settings.rotateLeft;
        rotateRight = settings.rotateRight;
    }

    private void PlaySFX(object sender, EventArgs e)
    {
        AudioManager.Main.RequestGUIFX(clearSFX);
    }

    public void initializeQEScheme()
    {
        rotateRight = KeyCode.Q;
        rotateLeft = KeyCode.E;

        rotationScheme = RotationControlScheme.QE;
    }

    private void TriggerMovement()
    {
        move?.Invoke();
    }

    private void KeyboardControlScheme()
    {
        Vector2 direction = new Vector2(
                    Utilities.TestKey(rightKey) - Utilities.TestKey(leftKey),
                    Utilities.TestKey(upKey) - Utilities.TestKey(downKey)
                    );

        OnMovementPressed?.Invoke(this, new MovementEventArgs(direction));          
    }

    private void MouseControlScheme()
    {
        var pos = Vector3.zero;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            pos = Input.mousePosition;
            pos -= new Vector3(720, 360, pos.z);
        }

        OnMovementPressed?.Invoke(this, new MovementEventArgs(pos.normalized));
    }

    public void SwitchMovementScheme(bool useMouse)
    {
        move = null;
        if(useMouse) move = MouseControlScheme;
        else move = KeyboardControlScheme;
    }

    private void TriggerRotation()
    {
        int direction = Utilities.TestKey(rotateRight) - Utilities.TestKey(rotateLeft);

        if(direction != 0)
        {
            OnRotationPressed?.Invoke(this, new RotationEventArgs(direction));
        }

        if(direction == 0)
        {
            OnInertia?.Invoke(this, EventArgs.Empty);
        }
    }

    private void WaveControl()
    {
        if(!GameManager.Main.onPause)
        {
            TriggerMovement();
            TriggerRotation();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }

    }

    private void RewardControl()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            OnSelectionClear?.Invoke(this, EventArgs.Empty);
        }
    }

    public void UnPause()
    {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    void Update()
    {
        if(GameManager.Main.gameState == GameState.OnWave) WaveControl();
        if(GameManager.Main.gameState == GameState.OnReward) RewardControl();
    }
}

public class MovementEventArgs : EventArgs
{
    public Vector2 direction;

    public MovementEventArgs(Vector2 direction)
    {
        this.direction = direction;
    }
}

public class RotationEventArgs : EventArgs
{
    public int direction;

    public RotationEventArgs(int direction)
    {
        this.direction = direction;
    }
}

