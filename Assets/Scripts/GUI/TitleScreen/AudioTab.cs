using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioTab : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider vfxSlider;
    [SerializeField] private Slider guiSlider;

    void Start()
    {
        audioManager = AudioManager.Main;
        musicSlider.value = audioManager.GetAudioTrack("Music").trackVolume;
        vfxSlider.value = audioManager.GetAudioTrack("SFX").trackVolume;
        guiSlider.value = audioManager.GetAudioTrack("GUI").trackVolume;
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
        track.trackVolume = volume;
    }

    public void HandleSFXVolume(float volume)
    {
        var track = audioManager.GetAudioTrack("SFX");
        track.trackVolume = volume;
    }

    public void HandleGUIVolume(float volume)
    {
        var track = audioManager.GetAudioTrack("GUI");
        track.trackVolume = volume;
    }
}
