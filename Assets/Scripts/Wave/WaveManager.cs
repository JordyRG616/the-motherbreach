using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] [FMODUnity.EventRef] private string onFormationSpawnSFX; 
    private UIAnimations endOfWaveAnimation;
    private ParticleSystem endOfWaveVFX;
    [SerializeField] [FMODUnity.EventRef] private string endOfWaveSFX;
    private Queue<WaveData> dataQueue  = new Queue<WaveData>();
    private WaveData activeWave;
    private ShipManager ship;
    private int spawnIndex;
    private List<FormationManager> activeFormations = new List<FormationManager>();

    public event EventHandler<EndWaveEventArgs> OnWaveEnd;
    private EndWaveEventArgs defaultArg = new EndWaveEventArgs();

    [ContextMenu("Initialize")]
    public void Initialize()
    {
        endOfWaveAnimation = GameObject.Find("End of wave text").GetComponent<UIAnimations>();
        endOfWaveVFX = GameObject.Find("End of wave VFX").GetComponent<ParticleSystem>();

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
        while(activeWave.breachQueue.Count > 0)
        {
            var breach = activeWave.breachQueue.Dequeue();
            breach.SetQueue();
            Vector2 spwPos = PositionToSpawn();

            while(breach.formationQueue.Count > 0)
            {
                if(!breach.spawnInSamePosition) spwPos = PositionToSpawn();
                var formation = Instantiate(breach.formationQueue.Dequeue(), spwPos, Quaternion.identity);

                activeFormations.Add(formation.GetComponent<FormationManager>());
                formation.GetComponent<FormationManager>().OnFormationDefeat += RemoveFormation;

                var formationPointer = Instantiate(pointer, Vector3.zero, Quaternion.identity);
                formationPointer.GetComponent<EnemyPointer>().ReceiveTarget(formation.transform.Find("Head"));
                AudioManager.Main.RequestSFX(onFormationSpawnSFX);

                yield return new WaitForSeconds(breach.intervalOfSpawn);
            }

            yield return new WaitForSeconds(breach.intervalTillNextWave);
        }

    }

    private void RemoveFormation(object sender, EventArgs e)
    {
        activeFormations.Remove(sender as FormationManager);
        if(activeFormations.Count == 0) StartCoroutine(EndWave());
    }

    private IEnumerator EndWave()
    {
        endOfWaveVFX.Play();
        // AudioManager.Main.RequestGUIFX(endOfWaveSFX);
        yield return StartCoroutine(endOfWaveAnimation.Forward());

        endOfWaveAnimation.PlayReverse();

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