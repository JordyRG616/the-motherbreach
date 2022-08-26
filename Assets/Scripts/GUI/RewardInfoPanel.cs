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
    private DataManager dataManager;
    private NodeMapManager mapManager;
    private UIAnimations anim;
    private bool firstOff = true;
    private int cashReward;
    private bool enablePack = true;

    void Awake()
    {
        anim = GetComponent<UIAnimations>();
        mapManager = NodeMapManager.Main;
        dataManager = DataManager.Main;
    }

    private void OpenMap()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(Open());
        }
    }

    private IEnumerator Open()
    {
        InputManager.Main.rewardShortcurt -= OpenMap;
        yield return anim.Forward();
        InputManager.Main.rewardShortcurt += CloseMap;
    }

    private void CloseMap()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(CloseInReward());
        }
    }

    private IEnumerator CloseInReward()
    {
        InputManager.Main.rewardShortcurt -= CloseMap;
        yield return anim.Reverse();
        InputManager.Main.rewardShortcurt += OpenMap;
    }

    public void Initiate(int cashReward, float time)
    {
        mapManager.FinishCurrentNode();
        instruction.SetActive(true);
        this.cashReward = cashReward;
        cashText.text = cashReward > 0 ? cashReward + "$" : "";
        if (time > 0) timeText.text = "Time: " + GetFormattedTime(time).minutes.ToString("00") + "m " + GetFormattedTime(time).seconds.ToString("00") + "s";
        else timeText.text = "";
        if (dataManager.SaveFileExists() && firstOff)
        {
            Invoke("Close", .75f);
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
        mapManager.SetAllNodesUnavailable();
        button.SetActive(false);
        if (firstOff)
        {
            if (!dataManager.SaveFileExists()) anim.PlayReverse();
            if(dataManager.SaveFileExists())
            {
                var choosing = System.BitConverter.ToBoolean(dataManager.saveFile.GetValue("ChoosingPack"));
                Debug.Log(choosing);
                if(choosing) PackOfferManager.Main.IniatiatePackChoice();
            }
            RewardManager.Main.InitiateReward(cashReward, true);
            InputManager.Main.rewardShortcurt += OpenMap;
            firstOff = false;
            return;
        }
        anim.PlayReverse();
        RewardCalculator.Main.InitiateChoice();
        RewardManager.Main.InitiateReward(cashReward, false);
    }

    private (int minutes, float seconds) GetFormattedTime(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = ((time / 60f) - minutes) * 60;

        return (minutes, seconds);
    }
}
