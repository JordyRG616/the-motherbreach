using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIAnimations : MonoBehaviour
{
    [SerializeField] protected float AnimationSpeed = 1;
    public float Speed 
    {
        get
        {
            return AnimationSpeed;
        }
    }
    
    protected RectTransform rect;
    protected WaitForSeconds waitTime = new WaitForSeconds(0.01f);

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

    protected abstract IEnumerator Forward();
    protected abstract IEnumerator Reverse();

}
