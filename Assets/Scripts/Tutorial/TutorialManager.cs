using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Vector2 skipWindowPosition;

    [Header("Tutorials")]
    [SerializeField] private List<TutorialStep> SelectionScreenTutorial;
    [SerializeField] private List<TutorialStep> initialTutorial;
    [SerializeField] private List<TutorialStep> posBuildTutorial;
    [SerializeField] private List<TutorialStep> waveTutorial;
    [SerializeField] private List<TutorialStep> upgradeTutorial;
    [SerializeField] private List<TutorialStep> lockTutorial;
    [SerializeField] private List<TutorialStep> rerollTutorial;
    [SerializeField] private List<TutorialStep> shopTutorial;
    [SerializeField] private List<TutorialStep> techTutorial;
    [SerializeField] private List<TutorialStep> statsTutorial;

    [Header("Components")]
    [SerializeField] private TutorialBox box;
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private RectTransform highlightPanel;
    public static bool tutorialFinished;

    public void ShowSkipWindow()
    {
        var text = "do you want to take the tutorial?";
        box.ReceiveTutorialInfo(skipWindowPosition, text, Direction.None, 1, true);
    }

    public void TriggerSelectionTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(SelectionScreenTutorial));
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
    }

    public void TriggerUpgradeTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(upgradeTutorial));
    }

    public void TriggerLockTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(lockTutorial));
    }

    public void TriggerRerollTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(rerollTutorial));
    }

    public void TriggerShopTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(shopTutorial));
    }

    public void TriggerTechTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(techTutorial));
    }

    public void TriggerStatTutorial()
    {
        if(tutorialFinished) return;
        StartCoroutine(ShowTutorial(statsTutorial));
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
            highlightPanel.anchoredPosition = step.panelPosition;
            highlightPanel.sizeDelta = step.panelSize;
            box.ReceiveTutorialInfo(step.boxPosition, step.stepText, step.arrowPosition, step.lines);

            yield return new WaitForSeconds(.2f);

            yield return new WaitUntil(() => Input.anyKeyDown);
        }

        box.Terminate();

        blockPanel.SetActive(false);
        steps.Clear();
    }

    public IEnumerator ShowWaveTutorial()
    {
        if(tutorialFinished) yield break;

        InputManager.Main.enabled = false;

        blockPanel.SetActive(true);

        for (int i = 0; i < waveTutorial.Count; i++)
        {
            var step = waveTutorial[i];
            box.ReceiveTutorialInfo(step.boxPosition, step.stepText, step.arrowPosition, step.lines);highlightPanel.anchoredPosition = step.panelPosition;
            highlightPanel.sizeDelta = step.panelSize;

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
    public Vector2 panelPosition;
    public Vector2 panelSize;
    public int lines;
    public Direction arrowPosition;    
    [TextArea] public string stepText;
}

