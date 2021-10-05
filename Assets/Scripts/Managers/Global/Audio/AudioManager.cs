using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if(_instance == null)
                {
                    _instance = new GameObject("Audio Manager").AddComponent<AudioManager>();
                }
            }

            return _instance;
        }
    }

    private AudioLibrary library;
    
    [SerializeField] private AudioTrack musicTrack;

    void Awake()
    {
        library = GetComponentInChildren<AudioLibrary>();
    }

    public void RequestMusic()
    {
        musicTrack.ReceiveAudio(library.GetMusic(), this);
    }

}
