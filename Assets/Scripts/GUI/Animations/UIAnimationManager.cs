using System.Text.RegularExpressions;
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
    private float elapsedTime;

    [ContextMenu("Play")]
    public void Play()
    {
        StartCoroutine(PlayTimeline());
    }

    [ContextMenu("Terminate")]
    public void Reverse()
    {
        StartCoroutine(ReverseTimeline());
        
    }

    private IEnumerator PlayTimeline()
    {
        foreach(AnimationGroup group in Timeline)
        {
            int i = 0;
            for(i = 0; i < group.animations.Count - 1; i++)
            {
                group.animations[i].Play();


                if(group.mode == AnimationGroup.PlayMode.Sequential)
                {
                    yield return new WaitForSecondsRealtime(group.interval);
                }
                
            }

            yield return StartCoroutine(group.animations[i].Forward());
        }

        if(interactablePanel) interactablePanel.SetActive(true);
    }

    private IEnumerator ReverseTimeline()
    {
        foreach(TurretSlotGUI slot in FindObjectsOfType<TurretSlotGUI>())
        {
            slot.DeactivateSprite();
        }


        var reverseTimeline = Timeline.Reverse<AnimationGroup>().ToList();

        foreach(AnimationGroup group in reverseTimeline)
        {
             int i = 0;
            
            foreach(UIAnimations animation in group.animations.Reverse<UIAnimations>())
            {
                if(animation.GetType() == typeof(CashTextAnimation)) continue;

                if(group.mode == AnimationGroup.PlayMode.Sequential && i > 0)
                {
                    yield return new WaitForSecondsRealtime(group.interval);
                }
                i++;
                animation.PlayReverse();
            }

            yield return new WaitUntil(() => group.IsDone());

        }

        if(interactablePanel) interactablePanel.SetActive(false);

        if(GameManager.Main.gameState == GameState.OnReward) RewardManager.Main.Exit();

    }

    internal void PlayGroup(object offerTimelineIndex)
    {
        throw new System.NotImplementedException();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void PlayGroup(int index)
    {
        var group = Timeline[index];

        _PlayGroup(group);
    }

    private IEnumerator _PlayGroup(AnimationGroup group)
    {
        int i = 0;
        for(i = 0; i < group.animations.Count - 1; i++)
        {
            group.animations[i].Play();


            if(group.mode == AnimationGroup.PlayMode.Sequential)
            {
                yield return new WaitForSecondsRealtime(group.interval);
            }
            
        }

        yield return StartCoroutine(group.animations[i].Forward());
    }
}

[System.Serializable]
public struct AnimationGroup
{
    public enum PlayMode {Simultaneous, Sequential}

    public PlayMode mode;
    public float interval;
    public List<UIAnimations> animations;

    public bool IsDone()
    {
        foreach(UIAnimations animation in animations)
        {
            if(animation.Done) return true;
        }
        return false;
    }
}