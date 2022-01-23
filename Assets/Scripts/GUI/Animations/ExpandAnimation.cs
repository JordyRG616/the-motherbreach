using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandAnimation : UIAnimations
{
    [SerializeField] private Vector2 TargetScale;
    private Vector2 ogScale;

    public override bool Done { get; protected set; }

    public override IEnumerator Forward()
    {
        float step = 0;
        int index = int.MaxValue;

        if(PlaySFX) AudioManager.Main.RequestGUIFX(OnStartSFX, out index);
        ogScale = rect.localScale;

        while(step <= 1 + (duration/100))
        {
            Vector2 _scale = Vector2.Lerp(ogScale, TargetScale, step);
            rect.localScale = _scale;
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        if(PlaySFX) AudioManager.Main.StopGUIFX(index);

        Done = true;
    }

    public override IEnumerator Reverse()
    {
        float step = 0;
        int index = int.MaxValue;

        if(PlayReverseSFX) AudioManager.Main.RequestGUIFX(OnReverseSFX, out index);

        while(step <= 1 + (duration/100))
        {
            Vector2 _scale = Vector2.Lerp(TargetScale, ogScale, step);
            rect.localScale = _scale;
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        if(PlayReverseSFX) AudioManager.Main.StopGUIFX(index);
    }
}
