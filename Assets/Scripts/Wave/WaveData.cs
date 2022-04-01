using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Wave", fileName ="New Wave Data")]
public class WaveData : ScriptableObject
{
    public List<string> enemiesInWave;
    [SerializeField] private List<Breach> breaches;
    public float rewardValue;

    public Queue<Breach> breachQueue = new Queue<Breach>();
 
    public void SetQueue()
    {
        breachQueue.Clear();
        foreach(Breach breach in breaches)
        {
            breachQueue.Enqueue(breach);
        }
    }

    public int GetBreachCount()
    {
        return breaches.Count;
    }
}

[System.Serializable]
public class Breach
{
    [SerializeField] private List<GameObject> availableFormations;
    public float intervalOfSpawn;
    public float intervalTillNextWave;
    public bool spawnInSamePosition;
    public bool bossWave;
    public int breachLevel;


    public Queue<GameObject> formationQueue = new Queue<GameObject>();

    public void SetQueue()
    {
        formationQueue.Clear();
        foreach(GameObject formation in availableFormations)
        {
            formationQueue.Enqueue(formation);
        }
    }
}