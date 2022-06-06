using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    public Dictionary<string, byte[]> GetData();

    public void LoadData(SaveFile saveFile);
}
