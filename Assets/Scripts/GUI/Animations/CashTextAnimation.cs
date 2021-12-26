using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashTextAnimation : UIAnimations
{
    [SerializeField] private TextMeshProUGUI inPocketCash;
    [SerializeField] private TextMeshProUGUI earnedCash;
    private RewardManager rewardManager;

    void Start()
    {
        rewardManager = RewardManager.Main;
    }

    protected override IEnumerator Forward()
    {
        
        earnedCash.gameObject.SetActive(true);

        earnedCash.text = "+ " + rewardManager.EarnedCash + "$";


        Color textColor  = earnedCash.color;
        textColor.a = 0;

        float step = 0;

        while(step <= 1)
        {
            earnedCash.color = textColor;
            textColor.a = Mathf.Lerp(0, 1, step);

            step += 0.01f;

            yield return new WaitForSecondsRealtime(0.01f);
            
        }

        step = rewardManager.EarnedCash;

        while(step > 0)
        {
            step --;
            rewardManager.TotalCash ++;

            earnedCash.text = "+ " + step + "$";
            inPocketCash.text = "cash = " + rewardManager.TotalCash + "$";



            yield return new WaitForSecondsRealtime(0.1f);
        }

        earnedCash.gameObject.SetActive(false);
    }

    protected override IEnumerator Reverse()
    {
        var ogTotal = rewardManager.TotalCash;
        rewardManager.TotalCash -= rewardManager.SpendedCash;

        earnedCash.gameObject.SetActive(true);

        earnedCash.text = "- " + rewardManager.SpendedCash + "$";

        Color textColor  = earnedCash.color;
        textColor.a = 0;

        float step = 0;

        while(step <= 1)
        {
            earnedCash.color = textColor;
            textColor.a = Mathf.Lerp(0, 1, step);

            step += 0.01f;

            yield return new WaitForSecondsRealtime(0.01f);
            
        }

        step = rewardManager.SpendedCash;

        while(step > 0)
        {
            
            step --;
            ogTotal --;

            earnedCash.text = "- " + step + "$";
            inPocketCash.text = "cash = " + ogTotal + "$";


            yield return new WaitForSecondsRealtime(0.1f);
        }

        earnedCash.gameObject.SetActive(false);
    }
}
