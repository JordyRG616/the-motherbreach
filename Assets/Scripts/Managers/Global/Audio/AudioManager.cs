using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMOD.Studio;

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
    
    [SerializeField] private Settings settings;
    [SerializeField] private AudioTrack musicTrack;
    [SerializeField] private AudioTrack uniqueMusicTrack;
    [SerializeField] private AudioTrack SFXTrack;
    [SerializeField] private AudioTrack GUITrack;
    [Header("Switch Track")]
    [SerializeField] private AnimationCurve risingCurve;
    [SerializeField] private AnimationCurve fallingCurve;

    void Awake()
    {
        if(_instance != null) Destroy(gameObject);
    }

    public void Initialize()
    {
        SetVolume();
        

        library = GetComponentInChildren<AudioLibrary>();
        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume()
    {
        musicTrack.Initiate();
        musicTrack.trackVolume = settings.musicVolume;
        uniqueMusicTrack.Initiate();
        uniqueMusicTrack.trackVolume = settings.musicVolume;
        SFXTrack.Initiate();
        SFXTrack.trackVolume = settings.sfxVolume;
        GUITrack.Initiate();
        GUITrack.trackVolume = settings.guiVolume;
    }

    public void RequestMusic()
    {
        musicTrack.ReceiveAudio(library.GetMusic());

        StartCoroutine(Playlist());
    }

    public void RequestMusic(string musicName)
    {
        uniqueMusicTrack.ReceiveAudio(library.GetMusic(musicName));
    }

    public void RequestBossMusic(string musicName, out FMOD.Studio.EventInstance instance)
    {
        StopCoroutine(Playlist());
        musicTrack.StopAllAudio();
        instance = library.GetMusic(musicName);
        musicTrack.ReceiveAudio(instance);
    }

    public float GetMusicVolume()
    {
        if(musicTrack.trackVolume > uniqueMusicTrack.trackVolume) return musicTrack.trackVolume;
        else return uniqueMusicTrack.trackVolume;
    }

    private IEnumerator Playlist()
    {
        yield return new WaitUntil(() => !musicTrack.AudioIsPlaying());

        RequestMusic();
    }

    /// <param name="track">Music or Special</param>
    public void SwitchMusicTracks(string track)
    {
        switch(track)
        {
            case "Music":
                StartCoroutine(SwitchToWaveTrack());
            break;
            case "Special":
                StartCoroutine(SwitchToRewardTrack());
            break; 
        }
    }

    private IEnumerator SwitchToWaveTrack()
    {
        musicTrack.UnpauseAudio();
        float step = 0;
        var volume = settings.musicVolume;

        while (step <=  1)
        {
            uniqueMusicTrack.trackVolume = fallingCurve.Evaluate(step) * volume;
            musicTrack.trackVolume = risingCurve.Evaluate(step) * volume;

            step += 0.01f;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        uniqueMusicTrack.trackVolume = 0;
        uniqueMusicTrack.PauseAudio();
        musicTrack.trackVolume = volume;
    }

    private IEnumerator SwitchToRewardTrack()
    {
        uniqueMusicTrack.UnpauseAudio();
        float step = 0;
        var volume = settings.musicVolume;

        while (step <=  1)
        {
            uniqueMusicTrack.trackVolume = fallingCurve.Evaluate(step) * volume;
            musicTrack.trackVolume = risingCurve.Evaluate(step) * volume;


            step += 0.01f;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        musicTrack.trackVolume = 0;
        musicTrack.PauseAudio();
        uniqueMusicTrack.trackVolume = volume;
    }

    public void RequestSFX(string eventPath)
    {
        SFXTrack.ReceiveAudio(library.GetSFX(eventPath));
    }
    
    public void RequestSFX(string eventPath, out FMOD.Studio.EventInstance instance)
    {
        instance = library.GetSFX(eventPath);
        SFXTrack.ReceiveAudio(instance);
    }

    public void StopSFX(FMOD.Studio.EventInstance instance)
    {
        SFXTrack.StopAudio(instance);
    }

    public bool IsPlayingSFX(FMOD.Studio.EventInstance instance)
    {
        return SFXTrack.activeChannels.Contains(instance);
    }

    public void RequestGUIFX(string eventPath, out int index)
    {
        GUITrack.ReceiveAudio(library.GetGUISound(eventPath));
        index = GUITrack.activeChannels.Count - 1;
    }

    public void RequestGUIFX(string eventPath)
    {
        GUITrack.ReceiveAudio(library.GetGUISound(eventPath));
    }

    public void StopGUIFX(int index)
    {
        GUITrack.StopAudio(index);
    }

    public void PlayInvalidSelection(string invalidTip)
    {
        if(GameManager.Main.gameState == GameState.OnReward || GameManager.Main.gameState == GameState.OnTitle)
        {
            InvalidPopup.Main.PopInvalidText(invalidTip);
            GUITrack.ReceiveAudio(library.GetGUISound("event:/UI/Reward/Reward_InvalidSelection"));
        }

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

    /// <param name="track">Music, Special, SFX or GUI</param>
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
            case "Special":
                return uniqueMusicTrack;
            default:
                return null;
        }
    }

    void OnDestroy()
    {
        StopAllAudio();
    }

    public void StopAllAudio()
    {
        musicTrack.StopAllAudio();
        uniqueMusicTrack.StopAllAudio();
        SFXTrack.StopAllAudio();
        GUITrack.StopAllAudio();
    }
}
