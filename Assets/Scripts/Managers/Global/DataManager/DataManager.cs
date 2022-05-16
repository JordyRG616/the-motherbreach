using System.Net.Mime;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using UnityEngine.Serialization;
using DUCK.Crypto;
using System.Text.RegularExpressions;

public class DataManager : MonoBehaviour
{
    #region Singleton
    private static DataManager _instance;
    public static DataManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<DataManager>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<DataManager>();
                }
            }

            return _instance;
        }
    }
    #endregion


    [SerializeField] private Settings settings;
    [SerializeField] private List<GameObject> AllShips;
    [SerializeField] private List<GameObject> AllPilots;
    public SaveFile saveFile;
    public MetaSaveFile metaProgressionSave;

    private List<ISavable> saveInterfaces = new List<ISavable>();

    private string directoryPath;
    private string filePath = "/save.save";
    private string metaFilePath = "/meta.save";
    private const string password = "motherbreach";

    void Start()
    {
        // directoryPath = Application.persistentDataPath + "/saves";
        directoryPath = Application.persistentDataPath + "/saves";
        
        metaProgressionSave = new MetaSaveFile(null, null, 0, 0, 0);

        saveInterfaces.Add(RewardManager.Main);
        saveInterfaces.Add(WaveManager.Main);
        saveInterfaces.Add(RewardCalculator.Main);
        saveInterfaces.Add(settings);
    }

    public void SaveData()
    {
        var ship = ShipManager.Main;
        if(ship != null && !saveInterfaces.Contains(ship)) saveInterfaces.Add(ship);

        saveFile = new SaveFile(GetSaveData());

        saveFile.rdmState = UnityEngine.Random.state;

        var jFile = JsonUtility.ToJson(saveFile);

        if(!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var _filePath = directoryPath + filePath;

        if(File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }

        var encrypt = SimpleAESEncryption.Encrypt(jFile, password);
        jFile = encrypt.EncryptedText + " " + encrypt.IV;

        using(FileStream stream = new FileStream(_filePath, FileMode.CreateNew))
        {
            using(StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(jFile);
                writer.Close();
            }
            stream.Close();
        }
    }

    public void SaveMetaData(List<int> pilots, List<int> ships, int powerCore, int reinforcedCore, int nobleCore)
    {
        metaProgressionSave = new MetaSaveFile(pilots, ships, powerCore, reinforcedCore, nobleCore);

        var jFile = JsonUtility.ToJson(metaProgressionSave);

        if(!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var _filePath = directoryPath + metaFilePath;

        if(File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }

        var encrypt = SimpleAESEncryption.Encrypt(jFile, password);
        jFile = encrypt.EncryptedText + " " + encrypt.IV;

        using(FileStream stream = new FileStream(_filePath, FileMode.CreateNew))
        {
            using(StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(jFile);
                writer.Close();
            }
            stream.Close();
        }
    }

    public void LoadMetaData()
    {     
        var _filePath = directoryPath + metaFilePath;

        if(File.Exists(_filePath))
        {
            using(FileStream stream = new FileStream(_filePath, FileMode.Open))
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    string[] split = text.Split(" ");

                    text = SimpleAESEncryption.Decrypt(split[0], split[1], password );

                    metaProgressionSave = JsonUtility.FromJson<MetaSaveFile>(text);

                    reader.Close();
                }
                stream.Close();
            }
        } else 
        {
            metaProgressionSave = null;
        }
    }

    public void LoadData()
    {
        var ship = ShipManager.Main;
        if(ship != null && !saveInterfaces.Contains(ship)) saveInterfaces.Add(ship);
        
        var _filePath = directoryPath + filePath;
        if(File.Exists(_filePath))
        {
            using(FileStream stream = new FileStream(_filePath, FileMode.Open))
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    string[] split = text.Split(" ");

                    text = SimpleAESEncryption.Decrypt(split[0], split[1], password );

                    saveFile = JsonUtility.FromJson<SaveFile>(text);

                    UnityEngine.Random.state = saveFile.rdmState;

                    SetSaveData();
                    reader.Close();
                }
                stream.Close();
            }
        } else 
        {
            saveFile = null;
        }
    }

    public bool SaveFileExists()
    {
        return File.Exists(directoryPath + filePath);
    }

    public void DeleteSaveFile()
    {
        if(File.Exists(directoryPath + filePath))
        {
            File.Delete(directoryPath + filePath);
            saveFile = null;
        }
    }

    public Dictionary<string, byte[]> GetSaveData()
    {
        var saveMatrix = new Dictionary<string, byte[]>();

        foreach(ISavable savable in saveInterfaces)
        {
            var data = savable.GetData();

            foreach(string id in data.Keys)
            {
                saveMatrix.Add(id, data[id]);
            }
        }

        return saveMatrix;
    }

    public void SetSaveData()
    {
        foreach(ISavable savable in saveInterfaces)
        {
            savable.LoadData(saveFile);
        }
    }

    public void SetSettingsData()
    {
        settings.LoadData(saveFile);
    }
    
    public GameObject GetLoadedShip()
    {
        var id = BitConverter.ToInt32(saveFile.GetValue("ShipIndex"));
        var loadedShip = AllShips.Find(x => x.GetComponent<ShipManager>().index == id);
        if(loadedShip == null) throw new Exception("Ship index not found");
        return loadedShip;
    }

    public GameObject GetLoadedPilot()
    {
        var id = BitConverter.ToInt32(saveFile.GetValue("PilotIndex"));
        var loadedPilot = AllPilots.Find(x => x.GetComponent<Pilot>().index == id);
        if(loadedPilot == null) throw new Exception("Pilot index not found");
        return loadedPilot;
    }
}
