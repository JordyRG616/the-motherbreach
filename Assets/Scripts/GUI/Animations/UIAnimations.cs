using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public abstract class UIAnimations : MonoBehaviour
{
    [FormerlySerializedAs("AnimationsSpeed")] [SerializeField] protected float duration = 1;
    [SerializeField] [Range(0.01f, 0.1f)] protected float animationSpeed = 0.01f;
    public abstract bool Done {get; protected set;}
    [SerializeField] protected bool PlaySFX;
    [SerializeField] [FMODUnity.EventRef] protected string OnStartSFX;
    [SerializeField] protected bool PlayReverseSFX;
    [SerializeField] [FMODUnity.EventRef] protected string OnReverseSFX;


    public float Speed 
    {
        get
        {
            return duration;
        }
    }
    
    protected RectTransform rect;
    protected WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.01f);

    protected virtual void Awake()
    {
        StopAllCoroutines();
        rect = GetComponent<RectTransform>();
    }

    public void Play()
    {
        StartCoroutine(Forward());
    }

    public void PlayReverse()
    {
        StartCoroutine(Reverse());
    }

    public abstract IEnumerator Forward();
    public abstract IEnumerator Reverse();

}
