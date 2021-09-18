using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Wave", fileName ="New Wave Data")]
public class WaveData : ScriptableObject
{
    public WaveType type;
    public int level;
    public List<GameObject> availableFormations;
}
