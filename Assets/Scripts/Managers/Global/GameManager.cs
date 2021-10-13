using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameState gameState {get; private set;} = GameState.OnReward;
    public event EventHandler<GameStateEventArgs> OnGameStateChange;
    private GameStateEventArgs toReward = new GameStateEventArgs(GameState.OnReward);
    private GameStateEventArgs toWave = new GameStateEventArgs(GameState.OnWave);

    private RewardManager rewardManager;
    private WaveManager waveManager;
    private InputManager inputManager;

    [ContextMenu("Start Game")]
    public void Start()
    {
        rewardManager = RewardManager.Main;
        rewardManager.Initialize();
        rewardManager.OnRewardSelection += InitiateWavePhase;

        waveManager = WaveManager.Main;
        waveManager.Initialize();
        waveManager.OnWaveEnd += InitiateRewardPhase;

        inputManager = InputManager.Main;
        OnGameStateChange += inputManager.HandleWaveControl;

        InitiateRewardPhase(this, EventArgs.Empty);
    }

    private void InitiateWavePhase(object sender, EventArgs e)
    {
        gameState = GameState.OnWave;
        OnGameStateChange?.Invoke(this, toWave);
        waveManager.StartNextWave();
    }

    private void InitiateRewardPhase(object sender, EventArgs e)
    {
        gameState = GameState.OnReward;
        OnGameStateChange?.Invoke(this, toReward);
        rewardManager.InitiateReward();
    }
}

public class GameStateEventArgs : EventArgs
{
    public GameState newState;

    public GameStateEventArgs(GameState newState)
    {
        this.newState = newState;
    }
}