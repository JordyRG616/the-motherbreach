using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Bases/Cheap Materials", fileName = "Cheap Material")]
public class CheapMaterials : Trait
{
    public override void ApplyEffect(Weapon weapon)
    {

    }

    public override string Description()
    {
        return "This base has no cost";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (CheapMaterials)ScriptableObject.CreateInstance(type);

        return instance;
    }
}
