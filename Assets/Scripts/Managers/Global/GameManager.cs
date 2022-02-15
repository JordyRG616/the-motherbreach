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


    private GameStateEventArgs toPause = new GameStateEventArgs(GameState.OnPause);

    private RewardManager rewardManager;
    private WaveManager waveManager;
    private InputManager inputManager;
    private AudioManager audioManager;

    [SerializeField] private UIAnimations fadePanelAnimation;
    private UIAnimationManager pauseAnimation;
    [SerializeField] private float initialCash;
    public bool onPause {get; private set;}

    void Start()
    {
        if(gameState == GameState.OnTitle)
        {
            audioManager = AudioManager.Main;
            audioManager.Initialize();
            audioManager.RequestMusic("Title");
        }
    }

    [ContextMenu("Start Game")]
    public void StartGameLoop()
    {
        DontDestroyOnLoad(gameObject);

        StartCoroutine(FadeScenes());
    }

    private IEnumerator FadeScenes()
    {
        yield return StartCoroutine(fadePanelAnimation.Forward());

        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += LateStart;

        // yield return StartCoroutine(fadePanelAnimation.Reverse());
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
        inputManager.OnGamePaused += HandleOptionsMenu;

        pauseAnimation = GameObject.FindGameObjectWithTag("PauseAnimation").GetComponent<UIAnimationManager>();

        // audioManager.RequestMusic();

        EndWaveEventArgs initialArgs = new EndWaveEventArgs();
        initialArgs.waveReward = initialCash;
        gameState = GameState.OnReward;
        InitiateRewardPhase(this, initialArgs);

        FindObjectOfType<TutorialManager>().ShowSkipWindow();

        SceneManager.sceneLoaded -= LateStart;
    }

    private void InitiateWavePhase(object sender, EventArgs e)
    {
        // globalVolume.SetActive(true);
        gameState = GameState.OnWave;
        OnGameStateChange?.Invoke(this, toWave);
        waveManager.StartNextWave();
    }

    private void InitiateRewardPhase(object sender, EndWaveEventArgs e)
    {
        // globalVolume.SetActive(false);
        gameState = GameState.OnReward;
        OnGameStateChange?.Invoke(this, toReward);
        rewardManager.InitiateReward(e.waveReward);
    }

    public void HandleOptionsMenu(object sender, EventArgs e)
    {
        if(onPause) StartCoroutine(Hide());
        
        else StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        onPause = true;
        audioManager.GetAudioTrack("SFX").PauseAudio();
        Time.timeScale = 0;
        yield return StartCoroutine(pauseAnimation.PlayTimeline());
    }


    private IEnumerator Hide()
    {
        onPause = false;
        audioManager.GetAudioTrack("SFX").UnpauseAudio();
        Time.timeScale = 1;
        yield return StartCoroutine(pauseAnimation.ReverseTimeline());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }
}

public class GameStateEventArgs : EventArgs
{
    public GameState newState;
    public EffectTrigger effectTrigger;

    public GameStateEventArgs(GameState newState)
    {
        this.newState = newState;
        if(newState == GameState.OnWave)
        {
            effectTrigger = EffectTrigger.StartOfWave;
        } else
        {
            effectTrigger = EffectTrigger.EndOfWave;
        }
    }
}