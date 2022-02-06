using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Wave", fileName ="New Wave Data")]
public class WaveData : ScriptableObject
{
    [SerializeField] private List<Breach> breaches;
    public float rewardValue;

    public Queue<Breach> breachQueue = new Queue<Breach>();
    // {
    //     get
    //     {
    //         var container = new Queue<Breach>();
    //         foreach(Breach breach in breaches)
    //         {
    //             container.Enqueue(breach);
    //         }
    //         return container;
    //     }
    // }

    public void SetQueue()
    {
        foreach(Breach breach in breaches)
        {
            breachQueue.Enqueue(breach);
        }
    }
}

[System.Serializable]
public struct Breach
{
    [SerializeField] private List<GameObject> availableFormations;
    public float intervalOfSpawn;
    public float intervalTillNextWave;
    public bool spawnInSamePosition;
    public bool bossWave;

    public Queue<GameObject> formationQueue;
    // {
    //     get
    //     {
    //         var container = new Queue<GameObject>();
    //         foreach(GameObject formation in availableFormations)
    //         {
    //             container.Enqueue(formation);
    //         }
    //         return container;
    //     }
    // }

    public void SetQueue()
    {
        formationQueue = new Queue<GameObject>();
        foreach(GameObject formation in availableFormations)
        {
            formationQueue.Enqueue(formation);
        }
    }
}