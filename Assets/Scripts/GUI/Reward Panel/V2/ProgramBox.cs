using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ProgramBox : MonoBehaviour
{
    [SerializeField] private Image programIcon;
    [SerializeField] private PlusButton plus;
    [SerializeField] private TextMeshProUGUI levelRequirement;
    private string description;
    private RectTransform statInfoBox;

    void Start()
    {
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(description);
        }
    }

    public void SetupFilledBox(Program program)
    {
        plus.Deactivate();
        levelRequirement.gameObject.SetActive(false);

        programIcon.sprite = program.sprite;

        description = program.Description() + "\n\n" + program.LevelDescription[program.level - 1];
    }

    public void SetupLockedBox(int levelToUnlock)
    {
        plus.Deactivate();
        programIcon.enabled = false;
        levelRequirement.gameObject.SetActive(true);

        levelRequirement.text = "LVL\n" + levelToUnlock;
        description = "unlocks at level " + levelToUnlock;
    }

    public void SetupAvailableBox(bool HasUpgradePoints)
    {
        if (HasUpgradePoints) plus.Activate();
        else plus.Deactivate();
        programIcon.enabled = false;
        levelRequirement.gameObject.SetActive(false);
    }
}
