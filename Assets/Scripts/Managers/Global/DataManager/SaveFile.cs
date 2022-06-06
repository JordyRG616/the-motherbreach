using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public List<SaveContent> contents = new List<SaveContent>();
    public Random.State rdmState;

    public SaveFile(Dictionary<string, byte[]> data)
    {
        foreach(string identifier in data.Keys)
        {
            var content = new SaveContent(identifier, data[identifier]);
            contents.Add(content);
        }
    }

    public bool ContainsSavedContent(string identifier)
    {
        var content = contents.Find(x => x.identifier == identifier);
        return content != null;
    }

    public byte[] GetValue(string identifier)
    {
        var content = contents.Find(x => x.identifier == identifier);
        if(content == default(SaveContent)) throw new System.Exception("could not find saved " + identifier);
        return content.valueInByte;
    }
}

[System.Serializable]
public class MetaSaveFile
{
    public List<int> unlockedPilotsIndices;
    public List<int> UnlockedShipsIndices;
    public int powerCoreAmount;
    public int reinforcedCoreAmount;
    public int nobleCoreAmount;
    
    public MetaSaveFile(List<int> unlockedPilots, List<int> unlockedShips, int powerCore, int reinforcedCore, int nobleCore)
    {
        unlockedPilotsIndices = unlockedPilots;
        UnlockedShipsIndices = unlockedShips;

        powerCoreAmount = powerCore;
        reinforcedCoreAmount = reinforcedCore;
        nobleCoreAmount = nobleCore;
    }
}

[System.Serializable]
public class SaveContent
{
    public string identifier;
    public byte[] valueInByte;

    public SaveContent(string identifier, byte[] value)
    {
        this.identifier = identifier;
        valueInByte = value;
    }
    
}
