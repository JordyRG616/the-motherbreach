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
    public List<Trait> programs;
    [TextArea] public string description;

    public void Initiate()
    {
        
    }
}

