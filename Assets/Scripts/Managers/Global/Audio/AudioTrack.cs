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
    private float _trackVolume
    {
        get
        {
            if(trackVolume < 0) return 0;
            if(trackVolume > 1) return 1;
            return trackVolume; 
        }
    }
    [SerializeField] private int maxChannels;
    [SerializeField] private MonoBehaviour invoker;
    public Dictionary<EventInstance, IEnumerator> activeChannels = new Dictionary<EventInstance, IEnumerator>();
    public bool paused {get; private set;}

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
            audioInstance.setVolume(_trackVolume);
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
            audioInstance.setVolume(_trackVolume);
            invoker.StartCoroutine(couroutine);
        }
    }

    private IEnumerator TrackAudioVolume(EventInstance audioInstance)
    {

        while (AudioIsPlaying(audioInstance))
        {
            audioInstance.setVolume(_trackVolume);

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
        if(!activeChannels.Keys.Contains(audioInstance)) return;
        activeChannels.Remove(audioInstance);
        audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        audioInstance.release();
    }

    public void StopAudio(int audioID)
    {
        if(activeChannels.Count > audioID)
        {
            EventInstance audioInstance = activeChannels.ElementAt(audioID).Key;
            activeChannels.Remove(audioInstance);
            audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            audioInstance.release();
        }
    }

    public void StopAllAudio(FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
    {
        activeChannels.Keys.ToList().ForEach(x => x.stop(mode));
        activeChannels.Clear();
    }

    public void PauseAudio()
    {
        foreach(EventInstance instance in activeChannels.Keys)
        {
            instance.setPaused(true);
        }
        paused = true;
    }

    
    public void UnpauseAudio()
    {
        foreach(EventInstance instance in activeChannels.Keys)
        {
            instance.setPaused(false);
        }
        paused = false;
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
