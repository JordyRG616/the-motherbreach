using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskAnimation : UIAnimations
{
    [SerializeField] private Vector2 targetSize;
    private RectMask2D mask;
    [SerializeField] private Vector2 ogSize;
    public override bool Done { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        mask = GetComponent<RectMask2D>();
    }

    public override IEnumerator Forward()
    {
        float step = 0;

        while (step <= 1 + (duration / 100))
        {
            Vector2 size = Vector2.Lerp(ogSize, targetSize, step);
            mask.padding = new Vector4(0, 0, size.x, size.y);
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        Done = true;
    }

    public override IEnumerator Reverse()
    {
        float step = 0;

        while (step <= 1 + (duration / 100))
        {
            Vector2 size = Vector2.Lerp(targetSize, ogSize, step);
            mask.padding = new Vector4(0, 0, size.x, size.y);
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();
    }
}
