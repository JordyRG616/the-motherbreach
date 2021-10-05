using FMODUnity;
using FMOD.Studio;

[System.Serializable]
public class AudioEventData
{
    public string eventName;
    [UnityEngine.SerializeField] private bool unique;
    [EventRef] public string eventPath;
    private bool instanceCreated = false;
    private EventInstance uniqueInstance;

    public EventInstance ReturnInstance()
    {
        if(unique)
        {
            if(instanceCreated == false)
            {
                uniqueInstance = RuntimeManager.CreateInstance(eventPath);
                instanceCreated = true;
            }

            return uniqueInstance;
        } 
        else
        {
            return RuntimeManager.CreateInstance(eventPath);
        }
    }
}
