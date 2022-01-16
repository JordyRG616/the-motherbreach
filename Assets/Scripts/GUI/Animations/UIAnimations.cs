using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIAnimations : MonoBehaviour
{
    [SerializeField] protected float AnimationSpeed = 1;
    public abstract bool Done {get; protected set;}
    [SerializeField] protected bool PlaySFX;
    [SerializeField] [FMODUnity.EventRef] protected string OnStartSFX;
    [SerializeField] protected bool PlayReverseSFX;
    [SerializeField] [FMODUnity.EventRef] protected string OnReverseSFX;


    public float Speed 
    {
        get
        {
            return AnimationSpeed;
        }
    }
    
    protected RectTransform rect;
    protected WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(0.01f);

    protected virtual void Awake()
    {
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
