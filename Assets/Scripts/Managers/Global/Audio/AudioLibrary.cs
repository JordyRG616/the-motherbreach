using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioLibrary : MonoBehaviour
{
    [SerializeField] private List<AudioEventData> musicEvents;
    [SerializeField] private List<AudioEventData> SFXEvents;
    [SerializeField] private List<AudioEventData> GUIEvents;

    public EventInstance GetMusic(string musicName)
    {
        return musicEvents.Find(x => x.eventName == musicName).ReturnInstance();
    }

    public EventInstance GetMusic()
    {
        int rdm = Random.Range(0, musicEvents.Count);
        return musicEvents[rdm].ReturnInstance();
    }


    public EventInstance GetSFX(string SFXName)
    {
        return SFXEvents.Find(x => x.eventPath == SFXName).ReturnInstance();
    }

    public EventInstance GetGUISound(string SFXName)
    {
        return GUIEvents.Find(x => x.eventPath == SFXName).ReturnInstance();
    }
}