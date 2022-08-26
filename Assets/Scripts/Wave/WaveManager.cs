using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using CustomRandom;

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
    private WaveData activeWave;
    private ShipManager ship;
    public List<FormationManager> activeFormations {get; private set;} = new List<FormationManager>();
    public List<BossController> activeBosses {get; private set;} = new List<BossController>();
    private TutorialManager tutorialManager;
    private ProgressionMeter progressionMeter;

    [SerializeField] private GameObject Jailship;
    private float jailshipChance;
    private float chanceIncrement = 0.01f;

    public event EventHandler<EndWaveEventArgs> OnWaveEnd;
    private EndWaveEventArgs defaultArg = new EndWaveEventArgs();

    public delegate void ApplyWaveModifier(FormationManager manager);
    public ApplyWaveModifier applyModifier;

    private int spawnIndex;
    private float waveTime;
    private bool counting;

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
        activeWave = null;

        endOfWaveAnimation = GameObject.Find("End of wave text").GetComponent<UIAnimations>();
        endOfWaveVFX = GameObject.Find("End of wave VFX").GetComponent<ParticleSystem>();

        tutorialManager = FindObjectOfType<TutorialManager>();
        progressionMeter = FindObjectOfType<ProgressionMeter>();

        AudioManager.Main.RequestMusic();

        ship = ShipManager.Main;
        listOfWaves = ScriptableObject.Instantiate(listOfWaves);
        listOfWaves.Initiate();
    }

    public WaveData GetRandomWaveByLevel(int level)
    {
        var list = listOfWaves.listsByLevel[level];
        var rdm = RandomManager.GetRandomInteger(0, list.Count);

        var wave = list[rdm];
        list.Remove(wave);
        return wave;
    }

    public WaveData GetRandomBossByLevel(int level)
    {
        var list = listOfWaves.bossesByLevel[level];
        var rdm = RandomManager.GetRandomInteger(0, list.Count);

        var wave = list[rdm];
        list.Remove(wave);
        return wave;
    }

    public EnemyInformation GetEnemyInformation(int index)
    {
        return listOfWaves.enemyInformation[index];
    }

    public BossInformation GetBossInformation(int index)
    {
        return listOfWaves.bossInformation[index];
    }

    public void StartNextWave()
    {
        GameManager.Main.SaveGame();
        applyModifier = null;
        FindObjectOfType<ShipController>().GetComponent<Rigidbody2D>().WakeUp();


        activeWave = NodeMapManager.Main.selectedNode.nodeWave;
        if(!NodeMapManager.Main.selectedNode.bossNode) applyModifier += NodeMapManager.Main.selectedNode.modifier.ApplyFormationEffect;
        activeWave.SetQueue();
        spawnIndex = activeWave.breachQueue.Count;
        counting = true;
        StartCoroutine(InstantiateFormations());
        
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
                    var boss = Instantiate(breach.GetRandomBoss(), spwPos, Quaternion.identity);

                    activeBosses.Add(boss.GetComponent<BossController>());
                    spawnIndex--;

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
                        applyModifier?.Invoke(manager);
                        spawnIndex--;

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
        if(GameManager.Main.GetPilotIndexToUnlock() == -1) return;

        var rdm = UnityEngine.Random.Range(0, 1f);

        Debug.Log(rdm + " / " + jailshipChance); 

        if(rdm > jailshipChance)
        {
            jailshipChance += chanceIncrement;
            return;
        } 

        var container = Instantiate(Jailship, position, Quaternion.identity);
        var formationPointer = Instantiate(pointer, container.transform.position, Quaternion.identity);
        formationPointer.GetComponent<EnemyPointer>().ReceiveTarget(container.transform);
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
        chanceIncrement += 0.01f;
        
        if(CheckForEndOfWave())
        {
            StartCoroutine(EndWave());
        } 
    }

    private bool CheckForEndOfWave()
    {
        if (spawnIndex > 0) return false;
        if (activeFormations.Count > 0) return false;
        return true;
    }

    private void Update()
    { 
        if(counting) waveTime += Time.deltaTime;
    }

    private IEnumerator EndWave()
    {
        endOfWaveVFX.Play();
        AudioManager.Main.RequestGUIFX(endOfWaveSFX);
        yield return StartCoroutine(endOfWaveAnimation.Forward());

        yield return StartCoroutine(endOfWaveAnimation.Reverse());

        yield return new WaitUntil(() => !endOfWaveVFX.IsAlive(true));

        AudioManager.Main.GetAudioTrack("SFX").PauseAudio();
        AudioManager.Main.SwitchMusicTracks("Special");

        if(NodeMapManager.Main.selectedNode.bossNode)
        {
            GameManager.Main.Win();
        }

        progressionMeter.AdvanceMarker();

        StopAllCoroutines();
        counting = false;
        defaultArg.waveReward = activeWave.rewardValue;
        defaultArg.waveTime = waveTime;
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
    public float waveTime;
}