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
    private Queue<WaveData> dataQueue  = new Queue<WaveData>();
    private WaveData activeWave;
    private ShipManager ship;
    private int spawnedFormations;

    public event EventHandler OnWaveEnd;


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

    public void StartNextWave()
    {
        if(dataQueue.Count > 0)
        {
            activeWave = dataQueue.Dequeue();
            spawnedFormations = 0;
            StartCoroutine(InstantiateFormations(PositionToSpawn()));
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

    private IEnumerator InstantiateFormations(Vector3 position)
    {
        while(spawnedFormations < activeWave.level)
        {
            int rdm = UnityEngine.Random.Range(0, activeWave.availableFormations.Count);
            var formationToSpawn = activeWave.availableFormations[rdm];

            GameObject formation = Instantiate(formationToSpawn, position, Quaternion.identity);
            var manager = formation.GetComponent<FormationManager>();
            manager.Initialize();
            manager.GetManager<PopulationManager>().RegisterToEvent(this);
            spawnedFormations++;

            yield return new WaitForSecondsRealtime(activeWave.intervalOfSpawn);
        }

        yield return new WaitUntil(() => FindObjectsOfType<FormationManager>().Length == 0);

        EndWave();
    }

    private void EndWave()
    {
        StopAllCoroutines();
        OnWaveEnd?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveLevel()
    {
        return activeWave.level;
    }

    public void ResetInstantiator(object sender, EventArgs e)
    {
        StartCoroutine(InstantiateFormations(PositionToSpawn()));
    }
}
