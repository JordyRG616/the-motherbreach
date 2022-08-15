using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AudioEffects
{
    private MonoBehaviour invoker;

    public AudioEffects(MonoBehaviour invoker)
    {
        this.invoker = invoker;
    }

    public void StartCrossfade(EventInstance musicOut, EventInstance musicIn, AudioTrack targetTrack)
    {
        invoker.StartCoroutine(Crossfade(musicOut, musicIn, targetTrack));
    }

    private IEnumerator Crossfade(EventInstance musicOut, EventInstance musicIn, AudioTrack targetTrack)
    {
        float step = 0;

        musicIn.setVolume(0);
        musicIn.start();


        while(step <= 1)
        {
            float fadeOut = Mathf.Lerp(1, 0, step * 2);
            musicOut.setVolume(fadeOut);

            float fadeIn = Mathf.Lerp(0, targetTrack.trackVolume, step);
            musicIn.setVolume(fadeIn);

            step += .01f;
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitUntil(() => step > 1);

        targetTrack.StopAudio(musicOut);

        invoker.StopCoroutine("Crossfade");
    }
    
}
