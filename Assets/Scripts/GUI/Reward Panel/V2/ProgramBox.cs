using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ProgramBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite filledSprite;
    [SerializeField] private Image programIcon;
    [SerializeField] private PlusButton plus;
    [SerializeField] private TextMeshProUGUI levelRequirement;

    [SerializeField] private Vector2 expandedSize;
    [SerializeField] private List<ProgramSelectionBox> selections;
    private Vector2 ogSize;
    private RectTransform self;

    private string description;
    private RectTransform statInfoBox;

    private TurretConstructor constructor;
    private BuildBox buildBox;
    private Image boxImage;

    private InputManager inputManager;

    void Start()
    {
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
        self = GetComponent<RectTransform>();
        ogSize = self.sizeDelta;

        constructor = TurretConstructor.Main;
        buildBox = FindObjectOfType<BuildBox>();

        boxImage = GetComponent<Image>();
        inputManager = InputManager.Main;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (description == "") return;
        if (!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statInfoBox.gameObject.SetActive(false);
    }


    public void SetupFilledBox(Trait program)
    {
        plus.Deactivate();
        levelRequirement.gameObject.SetActive(false);
        programIcon.enabled = true;

        programIcon.sprite = program.sprite;
        boxImage.sprite = filledSprite;

        description = program.Description();
    }

    public void SetupLockedBox(int levelToUnlock)
    {
        plus.Deactivate();
        programIcon.enabled = false;
        levelRequirement.gameObject.SetActive(true);

        boxImage.sprite = emptySprite;

        levelRequirement.text = "LVL\n" + levelToUnlock;
        description = "unlocks at level " + levelToUnlock;
    }

    public void SetupAvailableBox()
    {
        plus.Activate();
        programIcon.enabled = false;
        levelRequirement.gameObject.SetActive(false);
        boxImage.sprite = emptySprite;
    }

    public void EmptyBox()
    {
        plus.Deactivate();
        programIcon.enabled = false;
        levelRequirement.gameObject.SetActive(false);
        boxImage.sprite = emptySprite;
    }

    public void ToProgramSelection()
    {
        plus.Deactivate();
        StartCoroutine(Expand());
        inputManager.OnSelectionClear += Cancel;
        description = "";
    }

    private IEnumerator Expand()
    {
        boxImage.sprite = filledSprite;
        float step = 0;

        while (step <= 1)
        {
            var size = Vector2.Lerp(ogSize, expandedSize, step);
            self.sizeDelta = size;
            step += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        self.sizeDelta = expandedSize;

        SetupSelections();
    }

    private IEnumerator Shrink()
    {
        inputManager.OnSelectionClear -= Cancel;

        float step = 0;

        selections.ForEach(x => x.Clear());

        while (step <= 1)
        {
            var size = Vector2.Lerp(expandedSize, ogSize, step);
            self.sizeDelta = size;
            step += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        self.sizeDelta = ogSize;
    }

    private void SetupSelections()
    {
        for(int i = 0; i < selections.Count; i++)
        {
            var rdmProgram = constructor.GetRandomUnlockedProgram();
            selections[i].ReceiveProgram(rdmProgram);
        }
    }

    public void SelectProgram(Trait program)
    {
        var foundation = buildBox.selectedBase.GetComponent<Foundation>();
        //SetupFilledBox(program);
        foundation.ReceiveTrait(program);
        //foundation.GetComponentInParent<TurretManager>().upgradePoints--;
        buildBox.UpdateStats();
        StartCoroutine(Shrink());
    }

    private void Cancel(object sender, System.EventArgs e)
    {
        boxImage.sprite = emptySprite;
        StartCoroutine(Shrink());
    }
}
