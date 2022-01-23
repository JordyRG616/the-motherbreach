using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideAnimation : UIAnimations
{
    [SerializeField] private Vector2 distance;
    private Vector2 ogPosition;

    public override bool Done { get; protected set; }

    public override IEnumerator Forward()
    {
        int index = int.MaxValue;
        if(PlaySFX) AudioManager.Main.RequestGUIFX(OnStartSFX, out index);

        ogPosition = rect.anchoredPosition;

        float step = 0;

        while(step <= duration + (duration/100))
        {
            Vector2 _position = Vector2.Lerp(Vector2.zero, distance, step / duration);
            rect.anchoredPosition = _position + ogPosition;
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        if(PlaySFX) AudioManager.Main.StopGUIFX(index);

        Done = true;
    }

    public override IEnumerator Reverse()
    {
        int index = int.MaxValue;
        if(PlayReverseSFX) AudioManager.Main.RequestGUIFX(OnReverseSFX, out index);

        ogPosition = rect.anchoredPosition;

        float step = 0;

        while(step <= duration + (duration/100))
        {
            Vector2 _position = Vector2.Lerp(ogPosition, ogPosition - distance, step / duration);
            rect.anchoredPosition = _position;
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        if(PlayReverseSFX) AudioManager.Main.StopGUIFX(index);
    }
}
