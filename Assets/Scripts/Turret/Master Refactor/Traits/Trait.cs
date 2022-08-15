using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trait : ScriptableObject
{
    public int Id;
    public Sprite sprite;

    public virtual void Initiate(Weapon weapon) 
    {
        ApplyEffect(weapon);
    }

    public abstract void ApplyEffect(Weapon weapon);

    public virtual bool CheckRequirement()
    {
        return true;
    }

    public abstract string Description();

    public abstract Trait ReturnTraitInstance();
}
