using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private GameObject componentBox;
    private UIAnimations anim;
    private bool firstOff = true;

    void Awake()
    {
        anim = GetComponent<UIAnimations>();
    }

    public void Initiate(int cashReward)
    {
        if(firstOff)
        {
            RewardManager.Main.InitiateReward(cashReward);
            firstOff = false;
            return;
        }
        cashText.text = cashReward + "$";
        anim.Play();
        StartCoroutine(WaitForInput(cashReward));
    }

    private IEnumerator WaitForInput(int cashReward)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        RewardManager.Main.InitiateReward(cashReward);
        FindObjectOfType<LevelUpButton>().GainExp();
        anim.PlayReverse();
    }
}
