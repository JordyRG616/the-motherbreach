using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Nebula
{
    [SerializeField] private SpriteRenderer nebula;
    [SerializeField] private List<Material> possibleMaterials;

    public void SetRandomMaterial()
    {
        var rdm = UnityEngine.Random.Range(0, possibleMaterials.Count);
        var material = new Material(possibleMaterials[rdm]);

        nebula.material = material;
    }
}
