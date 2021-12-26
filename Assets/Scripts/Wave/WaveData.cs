using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Wave", fileName ="New Wave Data")]
public class WaveData : ScriptableObject
{
    public int level;
    public float intervalOfSpawn;
    public float rewardValue;
    public List<GameObject> availableFormations;
}
