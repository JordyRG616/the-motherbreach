using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DescriptionDictionary : MonoBehaviour
{
    #region Singleton
    private static DescriptionDictionary _instance;
    public static DescriptionDictionary Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<DescriptionDictionary>();
            }

            return _instance;
        }
    }
    #endregion


    [SerializeField] private TextAsset source;
    [SerializeField] private List<DictionaryEntry> entries;
    private EntryList entryList = new EntryList();


    void Awake()
    {
        entryList = JsonUtility.FromJson<EntryList>(source.text);
        entries = entryList.entries.ToList();
    }

    public string GetDescription(string entryName)
    {
        var entry = entries.Find(x => x.EntryName == entryName);
        
        if(entry != null)
        {
            return entry.EntryDescription;
        }

        else return "Entry was not found in the dictionary";
    }

}

[System.Serializable]
public class EntryList 
{
    public DictionaryEntry[] entries;
}

[System.Serializable]
public class DictionaryEntry
{
    public string EntryName;
    public string EntryDescription;
}
