using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName ="Data/Settings", fileName = "New Settings preset")]
public class Settings : ScriptableObject, ISavable
{
    [Range(0,1)] public float musicVolume;
    [Range(0,1)] public float sfxVolume;
    [Range(0,1)] public float guiVolume;

    public bool useMouse;
    public KeyCode rotateRight;
    public KeyCode rotateLeft;
    public KeyCode moveUp;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveDown;

    public bool fullscreen;
    public Vector2Int resolution;

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();

        container.Add("musicVolume", BitConverter.GetBytes(musicVolume));
        container.Add("sfxVolume", BitConverter.GetBytes(sfxVolume));
        container.Add("guiVolume", BitConverter.GetBytes(guiVolume));

        container.Add("useMouse", BitConverter.GetBytes(useMouse));
        container.Add("rotateRight", BitConverter.GetBytes(((int)rotateRight)));
        container.Add("rotateLeft", BitConverter.GetBytes(((int)rotateLeft)));
        container.Add("moveUp", BitConverter.GetBytes((int)moveUp));
        container.Add("moveLeft", BitConverter.GetBytes((int)moveLeft));
        container.Add("moveRight", BitConverter.GetBytes((int)moveRight));
        container.Add("moveDown", BitConverter.GetBytes((int)moveDown));

        container.Add("fullscreen", BitConverter.GetBytes(fullscreen));
        container.Add("resolutionX", BitConverter.GetBytes(resolution.x));
        container.Add("resolutionY", BitConverter.GetBytes(resolution.y));

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        musicVolume = BitConverter.ToSingle(saveFile.GetValue("musicVolume"));
        sfxVolume = BitConverter.ToSingle(saveFile.GetValue("sfxVolume"));
        guiVolume = BitConverter.ToSingle(saveFile.GetValue("guiVolume"));
        
        useMouse = BitConverter.ToBoolean(saveFile.GetValue("useMouse"));
        rotateLeft = (KeyCode)BitConverter.ToInt32(saveFile.GetValue("rotateLeft"));
        rotateRight = (KeyCode)BitConverter.ToInt32(saveFile.GetValue("rotateRight"));
        moveDown = (KeyCode)BitConverter.ToInt32(saveFile.GetValue("moveDown"));
        moveLeft = (KeyCode)BitConverter.ToInt32(saveFile.GetValue("moveLeft"));
        moveRight = (KeyCode)BitConverter.ToInt32(saveFile.GetValue("moveRight"));
        moveUp = (KeyCode)BitConverter.ToInt32(saveFile.GetValue("moveUp"));

        fullscreen = BitConverter.ToBoolean(saveFile.GetValue("fullscreen"));

        var resx = BitConverter.ToInt32(saveFile.GetValue("resolutionX"));
        var resy = BitConverter.ToInt32(saveFile.GetValue("resolutionY"));

    }
}
