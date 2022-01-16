using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashTextAnimation : UIAnimations
{
    [SerializeField] private TextMeshProUGUI inPocketCash;
    [SerializeField] private TextMeshProUGUI earnedCash;
    private RewardManager rewardManager;

    public override bool Done { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        rewardManager = RewardManager.Main;
    }

    public override IEnumerator Forward()
    {
        int index = int.MaxValue;


        
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

            yield return new WaitForSeconds(0.01f);
            
        }

        step = rewardManager.EarnedCash;

        if(PlaySFX) 
        {
            AudioManager.Main.RequestGUIFX(OnStartSFX, out index);
        }

        while(step > 0)
        {
            step --;
            rewardManager.TotalCash ++;

            earnedCash.text = "+ " + step + "$";
            inPocketCash.text = "cash = " + rewardManager.TotalCash + "$";



            yield return new WaitForSeconds(0.1f);
        }

        earnedCash.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();

        Done = true;

        if(PlaySFX) 
        {
            AudioManager.Main.StopGUIFX(index);
        }
    }

    public override IEnumerator Reverse()
    {
        int index = int.MaxValue;


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

            yield return new WaitForSeconds(0.01f);
            
        }

        step = rewardManager.SpendedCash;
        
        if(PlayReverseSFX) 
        {
            AudioManager.Main.RequestGUIFX(OnReverseSFX, out index);
        }

        while(step > 0)
        {
            
            step --;
            ogTotal --;

            earnedCash.text = "- " + step + "$";
            inPocketCash.text = "cash = " + ogTotal + "$";


            yield return new WaitForSeconds(0.1f);
        }

        earnedCash.gameObject.SetActive(false);

        if(PlayReverseSFX) 
        {
            AudioManager.Main.StopGUIFX(index);
        }
    }

    
}
