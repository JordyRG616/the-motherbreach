using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Wave List", fileName ="Wave List")]
public class WaveList : ScriptableObject
{
    public List<WaveData> waves;
}
