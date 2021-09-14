using System;
using CraftyUtilities;
using UnityEngine;

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


    [SerializeField] private MovementControlScheme movementScheme = MovementControlScheme.None;
    [SerializeField] private RotationControlScheme rotationScheme = RotationControlScheme.None;
    private KeyCode leftKey, rightKey, upKey, downKey; // MOVEMENT RELATED KEYCODES
    private KeyCode rotateRight, rotateLeft; // ROTATION RELATED KEYCODES

    public event EventHandler<MovementEventArgs> OnMovementPressed;
    public event EventHandler<RotationEventArgs> OnRotationPressed;

    void Awake()
    {
        if(movementScheme == MovementControlScheme.None || movementScheme == MovementControlScheme.WASD)
        {
            initializeWASDScheme();
        } 
        else if(movementScheme == MovementControlScheme.Arrows)
        {
            initializeArrowScheme();
        }

        if(rotationScheme == RotationControlScheme.None || rotationScheme == RotationControlScheme.QE)
        {
            initializeQEScheme();
        } 
        else if(rotationScheme ==  RotationControlScheme.Mouse)
        {
            initializeMouseScheme();
        }
    }

    private void initializeQEScheme()
    {
        rotateRight = KeyCode.Q;
        rotateLeft = KeyCode.E;
    }

    private void initializeMouseScheme()
    {
        rotateRight = KeyCode.Mouse0;
        rotateLeft = KeyCode.Mouse1;
    }

    private void initializeArrowScheme()
    {
        leftKey = KeyCode.LeftArrow;
        rightKey = KeyCode.RightArrow;
        upKey = KeyCode.UpArrow;
        downKey = KeyCode.DownArrow;
    }

    private void initializeWASDScheme()
    {
        leftKey = KeyCode.A;
        rightKey = KeyCode.D;
        upKey = KeyCode.W;
        downKey = KeyCode.S;
    }

    private void TriggerMovement()
    {
        Vector2 direction = new Vector2(
            Utilities.TestKey(rightKey) - Utilities.TestKey(leftKey),
            Utilities.TestKey(upKey) - Utilities.TestKey(downKey)
            );
        
        if(direction.magnitude != 0)
        {
            OnMovementPressed?.Invoke(this, new MovementEventArgs(direction));
        }
    }

    private void TriggerRotation()
    {
        int direction = Utilities.TestKey(rotateRight) - Utilities.TestKey(rotateLeft);

        if(direction != 0)
        {
            OnRotationPressed?.Invoke(this, new RotationEventArgs(direction));
        }
    }

    void Update()
    {
        TriggerMovement();
        TriggerRotation();
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

