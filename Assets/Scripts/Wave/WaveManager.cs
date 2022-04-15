using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WaveManager : MonoBehaviour, ISavable
{
    #region Singleton
    private static WaveManager _instance;
    public static WaveManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<WaveManager>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<WaveManager>();
                }
            }
            return _instance;
        }
    }
    #endregion


    [SerializeField] private WaveList listOfWaves;
    [SerializeField] private float distanceToSpawn;
    [SerializeField] private GameObject pointer;
    [SerializeField] private VisualEffect onFormationSpawnVFX;
    [SerializeField] [FMODUnity.EventRef] private string onFormationSpawnSFX; 
    private UIAnimations endOfWaveAnimation;
    private ParticleSystem endOfWaveVFX;
    [SerializeField] [FMODUnity.EventRef] private string endOfWaveSFX;
    private Queue<WaveData> dataQueue  = new Queue<WaveData>();
    private WaveData activeWave;
    private ShipManager ship;
    private int spawnIndex;
    private int waveIndex;
    public List<FormationManager> activeFormations {get; private set;} = new List<FormationManager>();
    public List<BossController> activeBosses {get; private set;} = new List<BossController>();
    private TutorialManager tutorialManager;
    private ProgressionMeter progressionMeter;

    [SerializeField] private GameObject Jailship;
    private float jailshipChance;

    public event EventHandler<EndWaveEventArgs> OnWaveEnd;
    private EndWaveEventArgs defaultArg = new EndWaveEventArgs();


    public void ClearEvents()
    {
        if(OnWaveEnd != null)
        {
            foreach(Delegate d in OnWaveEnd.GetInvocationList())
            {
                OnWaveEnd -= (EventHandler<EndWaveEventArgs>)d;
            }
        }
    }

    public void Initialize()
    {
        activeFormations.Clear();
        activeBosses.Clear();
        dataQueue.Clear();
        activeWave = null;

        endOfWaveAnimation = GameObject.Find("End of wave text").GetComponent<UIAnimations>();
        endOfWaveVFX = GameObject.Find("End of wave VFX").GetComponent<ParticleSystem>();

        tutorialManager = FindObjectOfType<TutorialManager>();
        progressionMeter = FindObjectOfType<ProgressionMeter>();

        AudioManager.Main.RequestMusic();

        ship = ShipManager.Main;

        GenerateDataQueue();
    }

    private void GenerateDataQueue()
    {
        dataQueue.Clear();
        activeWave = null;
        spawnIndex = 0;
        foreach(WaveData data in listOfWaves.waves)
        {
            dataQueue.Enqueue(data);
        }
    }

    public void StartNextWave()
    {
        FindObjectOfType<ShipController>().GetComponent<Rigidbody2D>().WakeUp();

        AudioManager.Main.GetAudioTrack("SFX").UnpauseAudio();
        AudioManager.Main.SwitchMusicTracks("Music");

        if(dataQueue.Count > 0)
        {
            activeWave = dataQueue.Dequeue();
            waveIndex ++;
            activeWave.SetQueue();
            spawnIndex = 0;
            StartCoroutine(InstantiateFormations());
        }
    }

    public WaveData GetNextWave()
    {
        var wave = dataQueue.Peek();
        return wave;
    }

    private Vector3 PositionToSpawn()
    {
        float rdmAngle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(rdmAngle), Mathf.Sin(rdmAngle)); 
        return (distanceToSpawn * direction) + ship.transform.position;
    }

    private IEnumerator InstantiateFormations()
    {
        yield return StartCoroutine(tutorialManager.ShowWaveTutorial());

        activeFormations.Clear();

        while(activeWave.breachQueue.Count > 0)
        {
            var breach = activeWave.breachQueue.Dequeue();
            breach.SetQueue();
            Vector2 spwPos = PositionToSpawn();
            if(breach.spawnInSamePosition)
            {
                StartCoroutine(CreateSpawnVFX(spwPos, 5));
                AudioManager.Main.RequestSFX(onFormationSpawnSFX);
                yield return new WaitForSeconds(1f);
            }

            if(breach.bossWave)
            {
                while(breach.formationQueue.Count > 0)
                {
                    if(!breach.spawnInSamePosition)
                    {
                        spwPos = PositionToSpawn();
                        StartCoroutine(CreateSpawnVFX(spwPos, 5, true));
                        yield return new WaitForSeconds(1f);
                    } 
                    var boss = Instantiate(breach.formationQueue.Dequeue(), spwPos, Quaternion.identity);

                    activeBosses.Add(boss.GetComponent<BossController>());

                    var formationPointer = Instantiate(pointer, boss.transform.position, Quaternion.identity);
                    formationPointer.GetComponent<EnemyPointer>().ReceiveTarget(boss.transform);
                }
            }
            else
            {
                while(breach.formationQueue.Count > 0)
                {
                    if(!breach.spawnInSamePosition) 
                    {
                        spwPos = PositionToSpawn();
                        StartCoroutine(CreateSpawnVFX(spwPos, 5));
                        AudioManager.Main.RequestSFX(onFormationSpawnSFX);
                        yield return new WaitForSeconds(1f);
                    }
                    var nextFormation = breach.formationQueue.Dequeue();
                    if(nextFormation != null)
                    {
                        CheckJailshipChance(-spwPos);
                        
                        var formation = Instantiate(nextFormation, spwPos, Quaternion.identity);
                        var manager = formation.GetComponent<FormationManager>();

                        activeFormations.Add(manager);
                        manager.formationLevel = breach.breachLevel;
                        manager.OnFormationDefeat += RemoveFormation;


                        var formationPointer = Instantiate(pointer, formation.transform.position, Quaternion.identity);
                        formationPointer.GetComponent<EnemyPointer>().ReceiveTarget(formation.transform.Find("Head"));
                    }

                    yield return new WaitForSeconds(breach.intervalOfSpawn);
                }
            }

            yield return new WaitForSeconds(breach.intervalTillNextWave);
        }

    }

    private void CheckJailshipChance(Vector3 position)
    {
        var rdm = UnityEngine.Random.Range(0, 1f);

        if(rdm > jailshipChance)
        {
            jailshipChance += 0.01f;
            return;
        } 

        var container = Instantiate(Jailship, position, Quaternion.identity);
        jailshipChance = 0;
    }

    private IEnumerator CreateSpawnVFX(Vector3 position, float duration, bool bossWave = false)
    {
        var container = Instantiate(onFormationSpawnVFX.gameObject, position, Quaternion.identity);
        if(bossWave) container.transform.localScale *= 1.5f;

        container.GetComponent<VisualEffect>().Play();

        yield return new WaitForSeconds(duration);

        container.GetComponent<VisualEffect>().Stop();

        yield return new WaitForSeconds(.5f);

        Destroy(container);
    }

    private void RemoveFormation(object sender, EventArgs e)
    {
        activeFormations.Remove(sender as FormationManager);
        if(CheckForEndOfWave())
        {
            StartCoroutine(EndWave());
        } 
    }

    internal void RemoveBoss(BossController bossController)
    {
        activeBosses.Remove(bossController);
        
        if(CheckForEndOfWave())
        {
            StartCoroutine(EndWave());
        } 
    }

    private bool CheckForEndOfWave()
    {
        return activeBosses.Count == 0 && activeFormations.Count == 0 && activeWave.breachQueue.Count == 0;
    }

    private IEnumerator EndWave()
    {
        endOfWaveVFX.Play();
        AudioManager.Main.RequestGUIFX(endOfWaveSFX);
        yield return StartCoroutine(endOfWaveAnimation.Forward());

        yield return StartCoroutine(endOfWaveAnimation.Reverse());

        yield return new WaitUntil(() => !endOfWaveVFX.IsAlive(true));

        if(dataQueue.Count == 0)
        {
            yield return new WaitForEndOfFrame();

            GameManager.Main.endgamePic = ScreenCapture.CaptureScreenshotAsTexture();

            GameManager.Main.Win();
        }

        progressionMeter.AdvanceMarker();

        StopAllCoroutines();
        defaultArg.waveReward = activeWave.rewardValue;
        OnWaveEnd?.Invoke(this, defaultArg);
    }

    public void ResetInstantiator(object sender, EventArgs e)
    {
        StartCoroutine(InstantiateFormations());
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();

        container.Add("waveIndex", BitConverter.GetBytes(waveIndex));

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        if(dataQueue.Count == 0) return;
        var count = BitConverter.ToInt32(saveFile.GetValue("waveIndex"));
        waveIndex = count;
        for (int i = 0; i < count; i++)
        {
            progressionMeter.AdvanceMarker();
            dataQueue.Dequeue();
        }
    }
}

public class EndWaveEventArgs : EventArgs
{
    public float waveReward;
}