using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : UIAnimations
{
    private Image image;
    [SerializeField] private float targetedAlpha = 1;

    public override bool Done { get; protected set; }

    protected override void Awake()
    {
        image = GetComponent<Image>();

        base.Awake();
    }

    public override IEnumerator Forward()
    {
        float step = 0;
        int index = int.MaxValue;

        if(PlaySFX) AudioManager.Main.RequestGUIFX(OnStartSFX, out index);
        Color color =  image.color;

        while(step <= 1 + (duration/100))
        {
            float _alpha = Mathf.Lerp(0, targetedAlpha, step);
            color.a = _alpha;
            image.color = color;
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
        Color color =  image.color;

        while(step <= 1 + (duration/100))
        {
            float _alpha = Mathf.Lerp(targetedAlpha, 0, step);
            color.a = _alpha;
            image.color = color;
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        if(PlayReverseSFX) AudioManager.Main.StopGUIFX(index);
    }
}
