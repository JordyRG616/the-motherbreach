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
        int index = int.MaxValue;

        if(PlaySFX) 
        {
            AudioManager.Main.RequestGUIFX(OnStartSFX, out index);
        }

        float step = 0;

        ogScale = rect.localScale;

        while(step <= 1 + (AnimationSpeed/100))
        {
            Vector2 _scale = Vector2.Lerp(ogScale, TargetScale, step);
            rect.localScale = _scale;
            step += AnimationSpeed / 100;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        Done = true;

        if(PlaySFX) 
        {
            AudioManager.Main.StopGUIFX(index);
        }

    }

    public override IEnumerator Reverse()
    {
        float step = 0;

        while(step <= 1 + (AnimationSpeed/100))
        {
            Vector2 _scale = Vector2.Lerp(TargetScale, ogScale, step);
            rect.localScale = _scale;
            step += AnimationSpeed / 100;
            yield return waitTime;
        }
    }
}
