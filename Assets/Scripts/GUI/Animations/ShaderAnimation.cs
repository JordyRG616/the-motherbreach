using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderAnimation : UIAnimations
{
    public override bool Done { get; protected set; }
    [SerializeField] private string variableName;
    [SerializeField] private float maxTime;
    private Material _material;

    protected override void Awake()
    {
        base.Awake();
        _material = new Material(GetComponent<Image>().material);
        GetComponent<Image>().material = _material;
        _material.SetFloat(variableName, 0);

    }

    public override IEnumerator Forward()
    {
        float step = 0;
        int index = 0;

        if(PlaySFX) AudioManager.Main.RequestGUIFX(OnStartSFX, out index);

        while(step <= AnimationSpeed)
        {
            _material.SetFloat(variableName, step * maxTime / AnimationSpeed);

            step += AnimationSpeed / 100;

            yield return waitTime;
        }

    }

    public override IEnumerator Reverse()
    {
        yield return waitTime;
    }
}
