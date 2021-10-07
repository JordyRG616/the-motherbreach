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

    void Awake()
    {
        effects = new AudioEffects(this);
        library = GetComponentInChildren<AudioLibrary>();
    }

    [ContextMenu("Play")]
    public void RequestMusic()
    {
        musicTrack.ReceiveAudio(library.GetMusic(), true);
    }

    [ContextMenu("Crossfade")]
    public void CrossfadeMusics()
    {
        effects.StartCrossfade(musicTrack.activeChannels.Keys.FirstOrDefault(), library.GetMusic(), musicTrack);
    }
}
