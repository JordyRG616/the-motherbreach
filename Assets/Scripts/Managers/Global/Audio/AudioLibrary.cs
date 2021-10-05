using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioLibrary : MonoBehaviour
{
    [SerializeField] private List<AudioEventData> musicEvents;

    public EventInstance GetMusic(string musicName)
    {
        return musicEvents.Find(x => x.eventName == musicName).ReturnInstance();
    }

    public EventInstance GetMusic()
    {
        int rdm = Random.Range(0, musicEvents.Count);
        return musicEvents[rdm].ReturnInstance();
    }

}