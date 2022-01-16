using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public enum OptionTab {Gameplay, Audio, Video}

    [SerializeField] private UIAnimationManager OptionMenuAnimation;
    private GameplayTab gameplayPanel;
    private AudioTab audioPanel;
    private VideoTab videoPanel;
    public OptionTab activeTab {get; private set;}


    public void ToOptions()
    {
        OptionMenuAnimation.Play();
    }

    public void FromOptions()
    {
        OptionMenuAnimation.Reverse();
    }

    public void ActivateTab (OptionTab tab)
    {
        activeTab = tab;
        switch(activeTab)
        {
            case OptionTab.Gameplay:
            break;
            case OptionTab.Audio:
            break;
            case OptionTab.Video:
            break;
        }
    }

}
