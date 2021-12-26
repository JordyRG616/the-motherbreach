using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandAnimation : UIAnimations
{
    [SerializeField] private Vector2Int TargetScale;
    private Vector2 ogScale;

    protected override IEnumerator Forward()
    {
        float step = 0;

        ogScale = rect.localScale;

        while(step <= 1 + (AnimationSpeed/100))
        {
            Vector2 _scale = Vector2.Lerp(ogScale, TargetScale, step);
            rect.localScale = _scale;
            step += AnimationSpeed / 100;
            yield return waitTime;
        }
    }

    protected override IEnumerator Reverse()
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
