using System.Linq;
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
    [SerializeField] private MonoBehaviour invoker;
    public Dictionary<EventInstance, IEnumerator> activeChannels = new Dictionary<EventInstance, IEnumerator>();

    public void ReceiveAudio(EventInstance audioInstance, bool unique = false)
    {

        if(activeChannels.Count < maxChannels)
        {
            if(unique && activeChannels.Keys.Contains(audioInstance))
            {
                StopAudio(audioInstance);
            }

            IEnumerator couroutine = TrackAudioVolume(audioInstance);
            activeChannels.Add(audioInstance, couroutine);
            audioInstance.setVolume(trackVolume);
            audioInstance.start();
            invoker.StartCoroutine(couroutine);
        }
    }

    public void ReceivePlayingAudio(EventInstance audioInstance)
    {
        if(activeChannels.Count < maxChannels)
        {
            IEnumerator couroutine = TrackAudioVolume(audioInstance);
            activeChannels.Add(audioInstance, couroutine);
            audioInstance.setVolume(trackVolume);
            invoker.StartCoroutine(couroutine);
        }
    }

    private IEnumerator TrackAudioVolume(EventInstance audioInstance)
    {

        while (AudioIsPlaying(audioInstance))
        {
            audioInstance.setVolume(trackVolume);

            yield return new WaitForEndOfFrame();
        }

        StopAudio(audioInstance);
    }

    public void StopTrackingVolume(EventInstance audioInstance)
    {
        if(activeChannels.Keys.Contains(audioInstance))
        {
            invoker.StopCoroutine(activeChannels[audioInstance]);
        }
    }

    public void StopAudio(EventInstance audioInstance)
    {
        audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        audioInstance.release();
        activeChannels.Remove(audioInstance);
    }

    public void StopAudio(int audioID)
    {
        if(activeChannels.Count > audioID)
        {
            EventInstance audioInstance = activeChannels.ElementAt(audioID).Key;
            audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            audioInstance.release();
            activeChannels.Remove(audioInstance);
        }
    }

    public bool AudioIsPlaying(EventInstance audioInstance)
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

    public bool AudioIsPlaying()
    {
        foreach(EventInstance audio in activeChannels.Keys)
        {
            if(AudioIsPlaying(audio))
            {
                return true;
            }
        }

        return false;
    }

}
