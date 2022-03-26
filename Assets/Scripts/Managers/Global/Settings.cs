using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Settings", fileName = "New Settings preset")]
public class Settings : ScriptableObject
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
}
