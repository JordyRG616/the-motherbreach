using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    public GameState gameState {get; private set;} = GameState.OnTitle;
    public event EventHandler<GameStateEventArgs> OnGameStateChange;
    private GameStateEventArgs toReward = new GameStateEventArgs(GameState.OnReward);
    private GameStateEventArgs toWave = new GameStateEventArgs(GameState.OnWave);

    private RewardManager rewardManager;
    private WaveManager waveManager;
    private InputManager inputManager;
    private AudioManager audioManager;

    [SerializeField] private GameObject globalVolume;
    [SerializeField] private float initialCash;


    [ContextMenu("Start Game")]
    public void StartGameLoop()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += LateStart;
    }

    private void LateStart(Scene scene, LoadSceneMode mode)
    {
        rewardManager = RewardManager.Main;
        rewardManager.Initialize();
        rewardManager.OnRewardSelection += InitiateWavePhase;

        waveManager = WaveManager.Main;
        waveManager.Initialize();
        waveManager.OnWaveEnd += InitiateRewardPhase;

        inputManager = InputManager.Main;
        OnGameStateChange += inputManager.HandleWaveControl;

        audioManager = AudioManager.Main;
        audioManager.Initialize();
        audioManager.RequestMusic();

        EndWaveEventArgs initialArgs = new EndWaveEventArgs();
        initialArgs.waveReward = initialCash;
        gameState = GameState.OnReward;
        InitiateRewardPhase(this, initialArgs);

        SceneManager.sceneLoaded -= LateStart;
    }

    private void InitiateWavePhase(object sender, EventArgs e)
    {
        // globalVolume.SetActive(true);
        gameState = GameState.OnWave;
        OnGameStateChange?.Invoke(this, toWave);
        waveManager.StartNextWave();
        audioManager.HandleMusicVolume(0.7f);
    }

    private void InitiateRewardPhase(object sender, EndWaveEventArgs e)
    {
        // globalVolume.SetActive(false);
        gameState = GameState.OnReward;
        OnGameStateChange?.Invoke(this, toReward);
        rewardManager.InitiateReward(e.waveReward);
        audioManager.HandleMusicVolume(-0.7f);
    }

    public void ShowOptionsMenu()
    {
        Debug.Log("options menu not implemented");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public class GameStateEventArgs : EventArgs
{
    public GameState newState;
    public BaseEffectTrigger effectTrigger;

    public GameStateEventArgs(GameState newState)
    {
        this.newState = newState;
        if(newState == GameState.OnWave)
        {
            effectTrigger = BaseEffectTrigger.StartOfWave;
        } else
        {
            effectTrigger = BaseEffectTrigger.EndOfWave;
        }
    }
}