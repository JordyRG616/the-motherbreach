using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViciousEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float buffPercentage;
    [SerializeField] private float durationOfBuff;
    private bool buffOn;
    private float timer;
    private bool counting;

    public override void ApplyEffect()
    {
        if(!buffOn && timer >= 1f)
        {
            timer = 0;
            counting = false;
            StartCoroutine(ManageBuff());
        }
    }

    private IEnumerator ManageBuff()
    {
        buffOn = true;
        var shooters = associatedController.GetShooters();
        float[] ogValues = new float[shooters.Count];

        for(int i = 0; i < shooters.Count; i++)
        {
            var ogValue = shooters[i].StatSet[Stat.Damage];
            ogValues[i] = ogValue;
            shooters[i].SetStat(Stat.Damage, ogValue * 1.5f);
        }

        yield return new WaitForSeconds(durationOfBuff);

        for(int i = 0; i < shooters.Count; i++)
        {
            shooters[i].SetStat(Stat.Damage, ogValues[i]);
        }

        buffOn = false;
    }
    
    public override string DescriptionText()
    {
        string description = "this turret gains " + buffPercentage * 100 + "% extra damage for " + durationOfBuff + " seconds";
        return description;
    }

    void FixedUpdate()
    {
        if(counting) timer += Time.fixedDeltaTime;
    }
}
