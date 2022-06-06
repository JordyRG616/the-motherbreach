using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Reward/Reward Pack", fileName ="New reward pack")]
public class Pack : ScriptableObject
{
    public int index;
    public List<int> requiredIndexes;
    public Sprite icon;
    public List<GameObject> rewards;
    public List<GameObject> possibleBases;
    public List<ShipSubroutine> possibleSubroutines;
    [HideInInspector] public ShipSubroutine selectedSubroutine;
    [TextArea] public string description;

    public void Initiate()
    {
        var rdm = Random.Range(0, possibleSubroutines.Count);
        selectedSubroutine = possibleSubroutines[rdm];
        selectedSubroutine.Initiate();
    }
}

