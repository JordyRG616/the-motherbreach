using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioTab : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private Settings settings;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider vfxSlider;
    [SerializeField] private Slider guiSlider;

    void Start()
    {
        audioManager = AudioManager.Main;
        musicSlider.value = settings.musicVolume;
        vfxSlider.value = settings.sfxVolume;
        guiSlider.value = settings.guiVolume;

    }

    public void ShowTab()
    {
        gameObject.SetActive(true);
    }

    public void HideTab()
    {
        gameObject.SetActive(false);
    }

    public void HandleMusicVolume(float volume)
    {
        var track = audioManager.GetAudioTrack("Music");
        var secondTrack = audioManager.GetAudioTrack("Special");
        secondTrack.trackVolume = volume;
        track.trackVolume = volume;
        settings.musicVolume = volume;
    }

    public void HandleSFXVolume(float volume)
    {
        var track = audioManager.GetAudioTrack("SFX");
        track.trackVolume = volume;
        settings.sfxVolume = volume;
    }

    public void HandleGUIVolume(float volume)
    {
        var track = audioManager.GetAudioTrack("GUI");
        track.trackVolume = volume;
        settings.guiVolume = volume;
    }
}
