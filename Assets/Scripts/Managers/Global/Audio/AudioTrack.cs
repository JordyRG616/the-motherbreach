using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


[System.Serializable]
public class AudioTrack 
{
    [Range(0, 1f)] public float trackVolume;
    [SerializeField] private int maxChannels;
    public List<EventInstance> activeChannels = new List<EventInstance>();

    public void ReceiveAudio(EventInstance audioInstance, MonoBehaviour emitter)
    {
        if(activeChannels.Count < maxChannels)
        {
            activeChannels.Add(audioInstance);
            audioInstance.setVolume(trackVolume);
            emitter.StartCoroutine(PlayAudio(audioInstance));
        }
    }

    private IEnumerator PlayAudio(EventInstance audioInstance)
    {
        audioInstance.start();

        while(AudioIsPlaying(audioInstance))
        {
            audioInstance.setVolume(trackVolume);

            yield return new WaitForEndOfFrame();
        }

        audioInstance.release();
        activeChannels.Remove(audioInstance);
    }

    private bool AudioIsPlaying(EventInstance audioInstance)
    {
        audioInstance.getPlaybackState(out PLAYBACK_STATE state);
        if(state == PLAYBACK_STATE.STOPPED)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
