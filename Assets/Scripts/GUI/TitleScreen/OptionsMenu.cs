using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void ExitGame()
    {
        GameManager.Main.ExitGame();
    }

    public void BackToTitle()
    {
        Destroy(GameManager.Main.gameObject);
        Destroy(AudioManager.Main.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void UnPause()
    {
        InputManager.Main.UnPause();
    }

}
