using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    #region Singleton
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
    #endregion

    private AudioLibrary library;
    private AudioEffects effects;
    
    [SerializeField] private AudioTrack musicTrack;
    [SerializeField] private AudioTrack SFXTrack;
    [SerializeField] private AudioTrack GUITrack;

    void Awake()
    {
        if(_instance != null) Destroy(gameObject);
    }

    public void Initialize()
    {
        effects = new AudioEffects(this);
        library = GetComponentInChildren<AudioLibrary>();
        DontDestroyOnLoad(gameObject);
    }

    public void RequestMusic()
    {
        // if(musicTrack.AudioIsPlaying()) CrossfadeMusics();
        // else 
        musicTrack.ReceiveAudio(library.GetMusic(), true);

        StartCoroutine(Playlist());
    }

    public void RequestMusic(string musicName)
    {
        // if(musicTrack.AudioIsPlaying()) CrossfadeMusics(musicName);
        // else 
        musicTrack.ReceiveAudio(library.GetMusic(musicName), true);
    }

    private IEnumerator Playlist()
    {
        yield return new WaitUntil(() => !musicTrack.AudioIsPlaying());

        RequestMusic();
    }

    public void StopMusicTrack()
    {
        musicTrack.StopAudio(0);
    }

    public void CrossfadeMusics()
    {
        effects.StartCrossfade(musicTrack.activeChannels.Keys.FirstOrDefault(), library.GetMusic(), musicTrack);
    }

    public void CrossfadeMusics(string musicName)
    {
        effects.StartCrossfade(musicTrack.activeChannels.Keys.FirstOrDefault(), library.GetMusic(musicName), musicTrack);
    }

    public void RequestSFX(string eventPath)
    {
        SFXTrack.ReceiveAudio(library.GetSFX(eventPath));
    }

    public void StopSFX(string eventPath)
    {
        SFXTrack.StopAudio(library.GetSFX(eventPath));
    }

    public void RequestGUIFX(string eventPath, out int index)
    {
        GUITrack.ReceiveAudio(library.GetGUISound(eventPath), true);
        index = GUITrack.activeChannels.Count - 1;
    }

    public void RequestGUIFX(string eventPath)
    {
        GUITrack.ReceiveAudio(library.GetGUISound(eventPath), true);
    }

    public void StopGUIFX(int index)
    {
        GUITrack.StopAudio(index);
    }

    public void PlayInvalidSelection()
    {
        GUITrack.ReceiveAudio(library.GetGUISound("event:/UI/Reward/Reward_InvalidSelection"), true);
    }

    public void HandleMusicVolume(float amount)
    {
        StartCoroutine(LerpMusicVolume(amount));
    }

    public IEnumerator LerpMusicVolume(float amount)
    {
        float step = 0;

        float og = musicTrack.trackVolume;
        float target = musicTrack.trackVolume + amount;

        while(step <= 1)
        {
            float newValue = Mathf.Lerp(og, target, step);

            musicTrack.trackVolume = newValue;

            step += 0.01f;

            yield return new WaitForSeconds(.01f);
        }
    }


    /// <param name="track">Music, SFX or GUI</param>
    public AudioTrack GetAudioTrack(string track)
    {
        switch(track)
        {
            case "Music":
                return musicTrack;
            case "SFX":
                return SFXTrack;
            case "GUI":
                return GUITrack;
            default:
                return null;
        }
    }
}
