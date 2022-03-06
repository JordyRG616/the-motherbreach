using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WaveManager : MonoBehaviour
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
    public List<FormationManager> activeFormations {get; private set;} = new List<FormationManager>();
    public List<BossController> activeBosses {get; private set;} = new List<BossController>();
    private TutorialManager tutorialManager;

    public event EventHandler<EndWaveEventArgs> OnWaveEnd;
    private EndWaveEventArgs defaultArg = new EndWaveEventArgs();


    public void Initialize()
    {
        endOfWaveAnimation = GameObject.Find("End of wave text").GetComponent<UIAnimations>();
        endOfWaveVFX = GameObject.Find("End of wave VFX").GetComponent<ParticleSystem>();

        tutorialManager = FindObjectOfType<TutorialManager>();

        AudioManager.Main.RequestMusic();

        ship = ShipManager.Main;
        GenerateDataQueue();
    }

    private void GenerateDataQueue()
    {
        foreach(WaveData data in listOfWaves.waves)
        {
            dataQueue.Enqueue(data);
        }
    }

    [ContextMenu("Next")]
    public void StartNextWave()
    {
        FindObjectOfType<ShipController>().GetComponent<Rigidbody2D>().WakeUp();

        if(dataQueue.Count > 0)
        {
            activeWave = dataQueue.Dequeue();
            activeWave.SetQueue();
            spawnIndex = 0;
            StartCoroutine(InstantiateFormations());
        }
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

        while(activeWave.breachQueue.Count > 0)
        {
            var breach = activeWave.breachQueue.Dequeue();
            breach.SetQueue();
            Vector2 spwPos = PositionToSpawn();
            if(breach.spawnInSamePosition)
            {
               StartCoroutine(CreateSpawnVFX(spwPos, breach.intervalOfSpawn * breach.formationQueue.Count));
            }

            if(breach.bossWave)
            {
                while(breach.formationQueue.Count > 0)
                {
                    if(!breach.spawnInSamePosition)
                    {
                        spwPos = PositionToSpawn();
                        StartCoroutine(CreateSpawnVFX(spwPos, breach.intervalOfSpawn));
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
                        StartCoroutine(CreateSpawnVFX(spwPos, breach.intervalOfSpawn));
                    } 
                    var formation = Instantiate(breach.formationQueue.Dequeue(), spwPos, Quaternion.identity);

                    activeFormations.Add(formation.GetComponent<FormationManager>());
                    formation.GetComponent<FormationManager>().OnFormationDefeat += RemoveFormation;

                    var formationPointer = Instantiate(pointer, formation.transform.position, Quaternion.identity);
                    formationPointer.GetComponent<EnemyPointer>().ReceiveTarget(formation.transform.Find("Head"));
                    AudioManager.Main.RequestSFX(onFormationSpawnSFX);

                    yield return new WaitForSeconds(breach.intervalOfSpawn);
                }
            }

            yield return new WaitForSeconds(breach.intervalTillNextWave);
        }

    }

    private IEnumerator CreateSpawnVFX(Vector3 position, float duration)
    {
        var container = Instantiate(onFormationSpawnVFX.gameObject, position, Quaternion.identity);

        container.GetComponent<VisualEffect>().Play();

        yield return new WaitForSeconds(duration);

        container.GetComponent<VisualEffect>().Stop();

        yield return new WaitForSeconds(.5f);

        Destroy(container);
    }

    private void RemoveFormation(object sender, EventArgs e)
    {
        activeFormations.Remove(sender as FormationManager);
        if(activeFormations.Count == 0 && activeBosses.Count == 0) StartCoroutine(EndWave());
    }

    internal void RemoveBoss(BossController bossController)
    {
        activeBosses.Remove(bossController);
        if(activeBosses.Count == 0 && activeFormations.Count == 0) StartCoroutine(EndWave());
    }

    private IEnumerator EndWave()
    {
        endOfWaveVFX.Play();
        // AudioManager.Main.StopMusicTrack();
        AudioManager.Main.RequestGUIFX(endOfWaveSFX);
        yield return StartCoroutine(endOfWaveAnimation.Forward());

        yield return StartCoroutine(endOfWaveAnimation.Reverse());

        StopAllCoroutines();
        defaultArg.waveReward = activeWave.rewardValue;
        OnWaveEnd?.Invoke(this, defaultArg);
    }

    public void ResetInstantiator(object sender, EventArgs e)
    {
        StartCoroutine(InstantiateFormations());
    }
    
}

public class EndWaveEventArgs : EventArgs
{
    public float waveReward;
}