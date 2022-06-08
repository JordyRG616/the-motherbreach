using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Program : ScriptableObject
{
    public int Id;
    public Sprite sprite;
    public int level;
    public List<string> LevelDescription;

    public abstract void ApplyEffect();
    public virtual bool CheckRequirement()
    {
        return true;
    }
    public abstract string Description();
}
