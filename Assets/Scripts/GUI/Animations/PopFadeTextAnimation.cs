using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopFadeTextAnimation : UIAnimations
{
    public override bool Done { get; protected set; }
    [SerializeField] private Vector2 popSize;
    [SerializeField] private float fadeAlpha;
    [SerializeField] private AnimationCurve curve;
    private TextMeshProUGUI textMesh;
    
    
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public override IEnumerator Forward()
    {
        float step = 0;
        int index = int.MaxValue;
        var ogSize = Vector2.zero;

        if(PlaySFX) AudioManager.Main.RequestGUIFX(OnStartSFX, out index);

        while(step <= duration)
        {
            var targetSize = Vector2.Lerp(ogSize, popSize, curve.Evaluate(step / duration));
            textMesh.rectTransform.localScale = targetSize;
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        if(PlaySFX) AudioManager.Main.StopGUIFX(index);

    }

    public override IEnumerator Reverse()
    {
        float step = 0;
        int index = int.MaxValue;
        var ogAlpha = 1;

        if(PlayReverseSFX) AudioManager.Main.RequestGUIFX(OnReverseSFX, out index);

        while(step <= duration)
        {
            var targetAlpha = Mathf.Lerp(ogAlpha, fadeAlpha, step / duration);
            textMesh.alpha = targetAlpha;
            step += animationSpeed;
            yield return waitTime;
        }

        yield return new WaitForEndOfFrame();

        if(PlayReverseSFX) AudioManager.Main.StopGUIFX(index);
    }
}
