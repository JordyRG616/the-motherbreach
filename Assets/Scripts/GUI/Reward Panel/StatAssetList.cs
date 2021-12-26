using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Stat Asset List", fileName = "New asset list")]
public class StatAssetList : ScriptableObject
{
    [SerializeField] private List<StatAssets> assets;

    public Sprite GetIcon(Stat stat)
    {
        var asset = assets.Find(x => x.stat == stat);
        return asset.icon;
    }
}

[System.Serializable]
public struct StatAssets
{
    public Stat stat;
    public Sprite icon;
}