using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


[System.Serializable]
public class AudioTrack 
{
    public float trackVolume
    {
        get => _trackVolume;
        set
        {
            if (value > 1) _trackVolume = 1;
            else if (value < 0) _trackVolume = 0;
            else _trackVolume = value;

            SetVolume(_trackVolume);
        }
    }
    
    private float _trackVolume;

    [SerializeField] private int maxChannels;
    public int currentChannels;
    public List<EventInstance> activeChannels { get; private set; } = new List<EventInstance>();
    private Bus bus;
    [SerializeField] private string busPath;
    public bool paused {get; private set;}
    private MonoBehaviour invoker;


    public void Initiate(MonoBehaviour invoker)
    {
        bus = RuntimeManager.GetBus(busPath);
        bus.setVolume(_trackVolume);
        this.invoker = invoker;
    }

    public void SetVolume(float newVolume)
    {
        bus.setVolume(newVolume);
    }

    public void ReceiveAudio(EventInstance audioInstance)
    {

        if(activeChannels.Count < maxChannels)
        {
            audioInstance.start();
            activeChannels.Add(audioInstance);
            currentChannels = activeChannels.Count;
            invoker.StartCoroutine(WatchInstance(audioInstance));
        }
    }

    private IEnumerator WatchInstance(EventInstance instance)
    {
        yield return new WaitUntil(() => InstanceStopped(instance));

        activeChannels.Remove(instance);
    }

    private bool InstanceStopped(EventInstance instance)
    {
        instance.getPlaybackState(out var state);
        if (state == PLAYBACK_STATE.STOPPED) return true;
        else return false;
    }

    public void StopAudio(EventInstance audioInstance)
    {
        if(!activeChannels.Contains(audioInstance)) return;
        activeChannels.Remove(audioInstance);
        audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        audioInstance.release();
    }

    public void StopAudio(int audioID)
    {
        if(activeChannels.Count > audioID)
        {
            EventInstance audioInstance = activeChannels[audioID];
            activeChannels.Remove(audioInstance);
            audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            audioInstance.release();
        }
    }

    public void StopAllAudio(FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
    {
        activeChannels.ForEach(x => x.stop(mode));
        activeChannels.Clear();
    }

    public void PauseAudio()
    {
        foreach(EventInstance instance in activeChannels)
        {
            instance.setPaused(true);
        }
        paused = true;
    }

    
    public void UnpauseAudio()
    {
        foreach(EventInstance instance in activeChannels)
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
        foreach(EventInstance audio in activeChannels)
        {
            if(AudioIsPlaying(audio))
            {
                return true;
            }
        }

        return false;
    }

}
