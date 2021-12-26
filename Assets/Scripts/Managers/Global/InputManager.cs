using System;
using System.Collections;
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
    public event EventHandler OnInertia;
    public event EventHandler OnSelectionClear;

    private IEnumerator _waveControl;
    private IEnumerator _rewardControl;


    public void Test()
    {
        StartCoroutine(_waveControl);
    }

    void Awake()
    {
        if (movementScheme == MovementControlScheme.None || movementScheme == MovementControlScheme.WASD)
        {
            initializeWASDScheme();
        }
        else if (movementScheme == MovementControlScheme.Arrows)
        {
            initializeArrowScheme();
        }

        if (rotationScheme == RotationControlScheme.None || rotationScheme == RotationControlScheme.QE)
        {
            initializeQEScheme();
        }
        else if (rotationScheme == RotationControlScheme.Mouse)
        {
            initializeMouseScheme();
        }

        _waveControl = WaveControl();
        _rewardControl = RewardControl();

        Test();

    }

    public void HandleWaveControl(object sender, GameStateEventArgs e)
    { 
        if(e.newState == GameState.OnWave)
        {
            StopAllCoroutines();
            StartCoroutine(_waveControl);
        }
        if(e.newState == GameState.OnReward)
        {
            StopAllCoroutines();
            StartCoroutine(_rewardControl);
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

        
        OnMovementPressed?.Invoke(this, new MovementEventArgs(direction));
        
        
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

    private IEnumerator WaveControl()
    {
        while(gameObject.activeSelf)
        {
            TriggerMovement();
            TriggerRotation();
            yield return new WaitForSecondsRealtime(0.001f);

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
            }
        }
    }

    private IEnumerator RewardControl()
    {
        while(gameObject.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Mouse1)) 
            {
                OnSelectionClear?.Invoke(this, EventArgs.Empty);
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
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

