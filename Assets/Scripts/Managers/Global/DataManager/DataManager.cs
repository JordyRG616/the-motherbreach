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
    [SerializeField] private Settings settings;
    private RewardManager rewardManager;
    private WaveManager waveManager;
    public SaveFile saveFile;

    private List<ISavable> saveInterfaces = new List<ISavable>();

    private string directoryPath = "E:/Unity Projects/#TURRENTBASE/PROJETO  - Base Invaders/saves";
    private string filePath = "/save.save";
    private const string password = "motherbreach";

    void Start()
    {
        rewardManager = RewardManager.Main;
        waveManager = WaveManager.Main;

        saveInterfaces.Add(rewardManager);
        saveInterfaces.Add(waveManager);
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
    
}
