using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        StartCoroutine(Playlist());
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

    public void RequestSFX(string eventPath)
    {
        SFXTrack.ReceiveAudio(library.GetSFX(eventPath));
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

            yield return new WaitForSecondsRealtime(.01f);
        }
    }
}
