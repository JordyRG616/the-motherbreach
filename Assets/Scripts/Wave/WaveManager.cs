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
            spawnIndex = 0;
            StartCoroutine(InstantiateFormations());
        } else
        { 
            Debug.Log("Victory");
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
        while(spawnIndex < activeWave.availableFormations.Count)
        {
            int qnt = UnityEngine.Random.Range(1, activeWave.level + 2);

            for(int i = 0; i < qnt; i++)
            {
                Vector2 spwPos = PositionToSpawn();
                var formation = Instantiate(activeWave.availableFormations[spawnIndex], spwPos, Quaternion.identity);

                activeFormations.Add(formation.GetComponent<FormationManager>());
                formation.GetComponent<FormationManager>().OnFormationDefeat += RemoveFormation;

                var formationPointer = Instantiate(pointer, Vector3.zero, Quaternion.identity);
                formationPointer.GetComponent<EnemyPointer>().ReceiveTarget(formation.transform);

                spawnIndex++;
                if(spawnIndex == activeWave.availableFormations.Count) break;

                yield return new WaitForSeconds(1f);
            }

            yield return new WaitForSeconds(activeWave.intervalOfSpawn);
        }

    }

    private void RemoveFormation(object sender, EventArgs e)
    {
        activeFormations.Remove(sender as FormationManager);
        if(activeFormations.Count == 0) EndWave();
    }

    private void EndWave()
    {
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