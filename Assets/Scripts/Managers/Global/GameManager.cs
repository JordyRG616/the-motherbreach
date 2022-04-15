using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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


    [SerializeField] private UIAnimations fadePanelAnimation;
    [SerializeField] private float initialCash;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject deleteSaveButton;
    
    [Header("Meta progression")]
    [SerializeField] private List<Pilot> unlockablePilots;
    private List<int> unlockedPilots = new List<int>();
    private List<int> unlockedShips = new List<int>();

    public GameState gameState {get; private set;} = GameState.OnTitle;
    public event EventHandler<GameStateEventArgs> OnGameStateChange;
    private GameStateEventArgs toReward = new GameStateEventArgs(GameState.OnReward);
    private GameStateEventArgs toWave = new GameStateEventArgs(GameState.OnWave);


    private GameStateEventArgs toPause = new GameStateEventArgs(GameState.OnPause);

    private RewardManager rewardManager;
    private RewardInfoPanel rewardInfoPanel;
    private WaveManager waveManager;
    private InputManager inputManager;
    private AudioManager audioManager;
    private PlanetSpawner planetSpawner;

    private GameObject selectedShip;
    private GameObject selectedPilot;

    private UIAnimationManager pauseAnimation;
    public bool onPause {get; private set;}

    private DataManager dataManager;
    private bool saveFound;

    public Texture2D endgamePic;

    void Start()
    {
        Application.targetFrameRate = 60;
        dataManager = GetComponent<DataManager>();
        LoadMetaData();

        if(dataManager.SaveFileExists())
        {
            dataManager.LoadData();
            dataManager.SetSettingsData();
            
            deleteSaveButton.SetActive(true);
            buttonText.text = "CONTINUE";
            saveFound = true;
        }

        if(gameState == GameState.OnTitle)
        {
            // Screen.SetResolution(1280, 720, true);
            inputManager = InputManager.Main;
            audioManager = AudioManager.Main;
            audioManager.Initialize();
            audioManager.RequestMusic("Title");
            planetSpawner = GetComponent<PlanetSpawner>();
            planetSpawner.Initialize();

            for(int i = 0; i < 3; i++)
            {
                planetSpawner.SpawnNewPlanet();
            }
        }
    }

    [ContextMenu("Start Game")]
    public void StartGameLoop()
    {
        DontDestroyOnLoad(gameObject);

        if(!saveFound) StartCoroutine(FadeToSelectionScreen());
        else StartCoroutine(FadeToGame());
    }

    private IEnumerator FadeToSelectionScreen()
    {
        fadePanelAnimation = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<FadeAnimation>();

        yield return StartCoroutine(fadePanelAnimation.Forward());

        SceneManager.LoadScene(4);
        // SceneManager.sceneLoaded += LateStart;

    }

    private IEnumerator FadeToGame()
    {
        fadePanelAnimation = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<FadeAnimation>();
        
        selectedShip = dataManager.GetLoadedShip();
        selectedPilot = dataManager.GetLoadedPilot();

        yield return StartCoroutine(fadePanelAnimation.Forward());

        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += LateStart;

    }

    public void StartGame(GameObject ship, GameObject pilot)
    {
        selectedShip = ship;
        selectedPilot = pilot;

        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += LateStart;
    }
    

    private void LateStart(Scene scene, LoadSceneMode mode)
    {
        InstantiateSelections();

        rewardManager = RewardManager.Main;
        rewardManager.Initialize();

        rewardInfoPanel = FindObjectOfType<RewardInfoPanel>();

        waveManager = WaveManager.Main;
        waveManager.Initialize();

        rewardManager.OnRewardSelection += InitiateWavePhase;
        waveManager.OnWaveEnd += InitiateRewardPhase;
        inputManager.OnGamePaused += HandleOptionsMenu;

        pauseAnimation = GameObject.FindGameObjectWithTag("PauseAnimation").GetComponent<UIAnimationManager>();

        if (dataManager.SaveFileExists())
        {
            LoadGame();
        }

        EndWaveEventArgs initialArgs = new EndWaveEventArgs();
        initialArgs.waveReward = dataManager.SaveFileExists() ? BitConverter.ToInt32(dataManager.saveFile.GetValue("totalCash")) : initialCash;
        gameState = GameState.OnReward;
        InitiateRewardPhase(this, initialArgs);

        FindObjectOfType<TutorialManager>().ShowSkipWindow();

        SceneManager.sceneLoaded -= LateStart;
    }

    private void InstantiateSelections()
    {
        var ship = Instantiate(selectedShip, Vector3.zero, Quaternion.identity);
        var portraitFrame = GameObject.FindGameObjectWithTag("PortraitFrame");
        var portrait = Instantiate(selectedPilot, Vector3.zero, Quaternion.identity, portraitFrame.transform);
        portrait.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 55);
        ship.GetComponent<ShipManager>().pilotIndex = portrait.GetComponent<Pilot>().index;
        portrait.GetComponent<Pilot>().Initialize();
    }

    private void GenerateBackground(int count)
    {
        planetSpawner.Initialize();

        for (int i = 0; i < count; i++)
        {
            var rdm = UnityEngine.Random.Range(20, 40);
            planetSpawner.distanceIncrement += rdm;
            planetSpawner.SpawnNewPlanet();
        }
    }

    private void InitiateWavePhase(object sender, EventArgs e)
    {
        gameState = GameState.OnWave;
        OnGameStateChange?.Invoke(this, toWave);
        waveManager.StartNextWave();
    }

    private void InitiateRewardPhase(object sender, EndWaveEventArgs e)
    {
        gameState = GameState.OnReward;
        OnGameStateChange?.Invoke(this, toReward);
        rewardInfoPanel.Initiate((int)e.waveReward);
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
        var menu = GameObject.FindWithTag("OptionMenu");

        // if(gameState == GameState.OnReward) menu.transform.localScale = new Vector3(.5f, .5f);
        // else menu.transform.localScale = new Vector3(1f, 1f);

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
        gameState = GameState.OnEndgame;

        Time.timeScale = 1;
        
        inputManager.ClearEvents();
        waveManager.ClearEvents();
        rewardManager.ClearEvents();

        foreach(Delegate d in OnGameStateChange.GetInvocationList())
        {
            OnGameStateChange -= (EventHandler<GameStateEventArgs>)d;
        }

        SceneManager.LoadScene(2);
    }

    public void Win()
    {
        gameState = GameState.OnEndgame;
        Time.timeScale = 1;

        // audioManager.StopAllAudio();
        // audioManager.RequestMusic("Victory");
        // audioManager.SwitchMusicTracks("Special");

        inputManager.ClearEvents();
        waveManager.ClearEvents();
        rewardManager.ClearEvents();

        foreach(Delegate d in OnGameStateChange.GetInvocationList())
        {
            OnGameStateChange -= (EventHandler<GameStateEventArgs>)d;
        }

        SceneManager.LoadScene(3);
    }

    public void SaveGame()
    {
        dataManager.SaveData();
    }

    private void LoadGame()
    {
        dataManager.LoadData();
        inputManager.SetKeys();
        audioManager.SetVolume();
    }

    public void DeleteSave()
    {
        dataManager.DeleteSaveFile();

        deleteSaveButton.SetActive(false);
        buttonText.text = "NEW GAME";
        saveFound = false;
    }

    public int GetPilotIndexToUnlock()
    {
        var rdm = UnityEngine.Random.Range(0, unlockablePilots.Count);
        return unlockablePilots[rdm].index;
    }

    public void UnlockPilot(int pilotIndex)
    {
        var pilot = unlockablePilots.Find(x => x.index == pilotIndex);
        unlockablePilots.Remove(pilot);
        unlockedPilots.Add(pilotIndex);
        SaveMetaData();
    }

    private void SaveMetaData()
    {
        dataManager.SaveMetaData(unlockedPilots, unlockedShips);
    }

    private void LoadMetaData()
    {
        dataManager.LoadMetaData();
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