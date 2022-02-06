using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialStep> initialTutorial;
    [SerializeField] private List<TutorialStep> posBuildTutorial;
    [SerializeField] private List<TutorialStep> waveTutorial;
    [SerializeField] private TutorialBox box;
    [SerializeField] private GameObject blockPanel;
    private bool tutorialFinished;
    

    public void ShowSkipWindow()
    {
        var text = "do you want to take the tutorial?";
        box.ReceiveTutorialInfo(new Vector2(336, -20), text, Direction.None, 1, true);
    }

    public void TriggerInitialTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(initialTutorial));
    }

    public void TriggerPosBuildTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(posBuildTutorial));
    }

    public void TriggerWaveTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(waveTutorial));
        tutorialFinished = true;
    }

    public void No()
    {
        tutorialFinished = true;
        blockPanel.SetActive(false);
        box.Terminate();
    }

    private IEnumerator ShowTutorial(List<TutorialStep> steps)
    {
        blockPanel.SetActive(true);

        for (int i = 0; i < steps.Count; i++)
        {
            var step = steps[i];
            box.ReceiveTutorialInfo(step.boxPosition, step.stepText, step.arrowPosition, step.lines);

            yield return new WaitForSeconds(.2f);

            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        box.Terminate();

        blockPanel.SetActive(false);
    }

    public IEnumerator ShowWaveTutorial()
    {
        if(tutorialFinished) yield break;

        InputManager.Main.enabled = false;

        blockPanel.SetActive(true);

        for (int i = 0; i < waveTutorial.Count; i++)
        {
            var step = waveTutorial[i];
            box.ReceiveTutorialInfo(step.boxPosition, step.stepText, step.arrowPosition, step.lines);

            yield return new WaitForSeconds(.2f);

            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        InputManager.Main.enabled = true;

        No();
    }
}

[System.Serializable]
public struct TutorialStep
{
    public Vector2 boxPosition;
    public int lines;
    public Direction arrowPosition;    
    [TextArea] public string stepText;
}

