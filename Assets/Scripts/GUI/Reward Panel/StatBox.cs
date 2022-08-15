using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class StatBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI header, value;
    private RectTransform statInfoBox;
    [SerializeField] private PlusButton plus;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    [HideInInspector] public string description;
    [SerializeField] private Image background;
    [SerializeField] private Color clickTextColor;

    private TurretStat stat;
    private List<StatBox> statBoxes = new List<StatBox>();
    private BuildBox buildBox;


    void Start()
    {
        statInfoBox = FindObjectOfType<StatInfoBox>(true).GetComponent<RectTransform>();
        statBoxes = FindObjectsOfType<StatBox>(true).ToList();

        buildBox = FindObjectOfType<BuildBox>();
    }

    public void Activate()
    {
        header.color = Color.white;
        value.color = Color.white;
        background.color = activeColor;
    }

    public void Deactivate()
    {
        header.color = Color.gray;
        value.color = Color.gray;
        background.color = inactiveColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!statInfoBox.gameObject.activeSelf)
        {
            statInfoBox.gameObject.SetActive(true);
            statInfoBox.GetComponent<StatInfoBox>().SetText(description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statInfoBox.gameObject.SetActive(false);
    }

    public void SetHeaderText(string text)
    {
        header.text = text.ToLower();
    }

    public void SetValueText(string text)
    {
        value.text = text.ToLower();
    }

    public void ReceiveStat(TurretStat stat)
    {
        this.stat = stat;
        CheckUpgradeButton();
    }

    public void CheckUpgradeButton()
    {
        if (stat == null) 
        {
            plus.Deactivate();
            return;
        }
        if (stat.CanUpgrade()) plus.Activate();
        else plus.Deactivate();
    }

    public void ApplyIncrement()
    {
        stat.incrementDelegate(stat.increment);
        statBoxes.ForEach(x => x.CheckUpgradeButton());

        header.color = clickTextColor;
        value.color = clickTextColor;

        buildBox.UpdateStats();
    }

    public void ResetGraphics()
    {
        header.color = Color.white;
        value.color = Color.white;
    }
}
