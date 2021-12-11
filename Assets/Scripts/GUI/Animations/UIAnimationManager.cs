using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationManager : MonoBehaviour
{
    #region Singleton
    private static UIAnimationManager _instance;
    public static UIAnimationManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<UIAnimationManager>();
            }

            return _instance;
        }
    }
    #endregion


    [SerializeField] private List<AnimationGroup> Timeline;
    [SerializeField] private GameObject interactablePanel;
    private WaitForSecondsRealtime sequentialTime = new WaitForSecondsRealtime(.5f);

    [ContextMenu("Play")]
    public void InitiateUI()
    {
        StartCoroutine(PlayTimeline());
    }

    [ContextMenu("Terminate")]
    public void TerminateUI()
    {
        StartCoroutine(ReverseTimeline());
        
    }

    private IEnumerator PlayTimeline()
    {
        foreach(AnimationGroup group in Timeline)
        {
            int i = 0;

            foreach(UIAnimations animation in group.animations)
            {
                if(group.mode == AnimationGroup.PlayMode.Sequential && i > 0)
                {
                    yield return sequentialTime;
                }
                i++;
                animation.Play();
            }

            yield return new WaitForSecondsRealtime(1 / group.GetLowestSpeed());
        }

        interactablePanel.SetActive(true);
    }

    private IEnumerator ReverseTimeline()
    {
        var reverseTimeline = Timeline.Reverse<AnimationGroup>().ToList();

        foreach(AnimationGroup group in reverseTimeline)
        {
             int i = 0;
            
            foreach(UIAnimations animation in group.animations.Reverse<UIAnimations>())
            {
                if(group.mode == AnimationGroup.PlayMode.Sequential && i > 0)
                {
                    yield return sequentialTime;
                }
                i++;
                animation.PlayReverse();
            }

            yield return new WaitForSecondsRealtime(1 / group.GetLowestSpeed());

        }
    }


}

[System.Serializable]
public struct AnimationGroup
{
    public enum PlayMode {Simultaneous, Sequential}

    public PlayMode mode;
    public List<UIAnimations> animations;

    public float GetLowestSpeed()
    {
        var orderedList = animations.OrderBy(x => x.Speed);
        return orderedList.FirstOrDefault().Speed;
    }
}