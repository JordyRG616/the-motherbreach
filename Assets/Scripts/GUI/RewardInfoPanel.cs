using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject instruction;
    private NodeMapManager mapManager;
    private UIAnimations anim;
    private bool firstOff = true;
    private int cashReward;

    void Awake()
    {
        anim = GetComponent<UIAnimations>();
        mapManager = NodeMapManager.Main;
    }

    public void Initiate(int cashReward, float time)
    {
        mapManager.FinishCurrentNode();
        button.SetActive(false);
        instruction.SetActive(true);
        this.cashReward = cashReward;
        cashText.text = cashReward > 0 ? cashReward + "$" : "";
        if (time > 0) timeText.text = "Time: " + GetFormattedTime(time).minutes.ToString("00") + "m " + GetFormattedTime(time).seconds.ToString("00") + "s";
        else timeText.text = "";
        if (DataManager.Main.SaveFileExists() && firstOff)
        {
            Invoke("Close", .5f);
            return;
        }
        anim.Play();
        StartCoroutine(WaitForInput());
    }

    private IEnumerator WaitForInput()
    {
        yield return new WaitUntil(() => mapManager.selectedNode != null);
        button.SetActive(true);
        instruction.SetActive(false);
    }

    public void Close()
    {
        mapManager.selectedNode.DeactivateLines();
        if (firstOff)
        {
            if (!DataManager.Main.SaveFileExists()) anim.PlayReverse();
            RewardManager.Main.InitiateReward(cashReward);
            firstOff = false;
            return;
        }
        anim.PlayReverse();
        RewardManager.Main.InitiateReward(cashReward);
        RewardCalculator.Main.InitiateChoice();
    }

    private (int minutes, float seconds) GetFormattedTime(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = ((time / 60f) - minutes) * 60;

        return (minutes, seconds);
    }
}
