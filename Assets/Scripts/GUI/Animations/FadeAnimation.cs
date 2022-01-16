using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : UIAnimations
{
    private Image image;

    public override bool Done { get; protected set; }

    protected override void Awake()
    {
        image = GetComponent<Image>();

        base.Awake();
    }

    public override IEnumerator Forward()
    {
        float step = 0;

        Color color =  image.color;

        while(step <= 1 + (AnimationSpeed/100))
        {
            float _alpha = Mathf.Lerp(0, 1, step);
            color.a = _alpha;
            image.color = color;
            step += AnimationSpeed / 100;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        Done = true;
    }

    public override IEnumerator Reverse()
    {
        float step = 0;

        Color color =  image.color;

        while(step <= 1 + (AnimationSpeed/100))
        {
            float _alpha = Mathf.Lerp(1, 0, step);
            color.a = _alpha;
            image.color = color;
            step += AnimationSpeed / 100;
            yield return waitTime;
        }
    }
}
