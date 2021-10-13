using System.Linq;
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
    private AudioEffects effects;
    
    [SerializeField] private AudioTrack musicTrack;
    [SerializeField] private AudioTrack SFXTrack;
    [SerializeField] private AudioTrack GUITrack;

    public void Initialize()
    {
        effects = new AudioEffects(this);
        library = GetComponentInChildren<AudioLibrary>();
    }

    public void RequestMusic()
    {
        musicTrack.ReceiveAudio(library.GetMusic(), true);
    }

    public void StopMusicTrack()
    {
        musicTrack.StopAudio(0);
    }

    public void CrossfadeMusics()
    {
        effects.StartCrossfade(musicTrack.activeChannels.Keys.FirstOrDefault(), library.GetMusic(), musicTrack);
    }

    public void RequestSFX(string eventPath)
    {
        SFXTrack.ReceiveAudio(library.GetSFX(eventPath));
    }
}
