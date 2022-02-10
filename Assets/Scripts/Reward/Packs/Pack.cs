using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Reward/Reward Pack", fileName ="New reward pack")]
public class Pack : ScriptableObject
{
    public int index;
    public Sprite icon;
    public List<GameObject> rewards;
    [TextArea] public string description;
}
