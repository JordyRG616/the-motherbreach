using System.Collections;
using StringHandler;
using UnityEngine;

public class ScrapperEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float percentage;
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] [FMODUnity.EventRef] private string sfx;
    private bool firstTime = true;

    public override void ApplyEffect()
    {
        foreach(ActionEffect shooter in associatedController.GetShooters())
        {
            foreach(Stat stat in targetedStats)
            {
                if(shooter.StatSet.ContainsKey(stat))
                {
                    var value = shooter.StatSet[stat];
                    value *= 1 + (percentage);
                    shooter.SetStat(stat, value);

                    vfx.Play();
                    Invoke("PlaySFX", 0.2f);
                }
            }
        }
    }

    private void PlaySFX()
    {
        AudioManager.Main.RequestSFX(sfx);
    }

    public override string DescriptionText()
    {
        string container = "";
        foreach(Stat stat in targetedStats)
        {
            container += stat.ToString().ToLower() + "/";
        }
        container = container.Remove(container.Length -1);
        var value = (percentage * 100).ToString() +"%" ;
        return "raises " + StatColorHandler.StatPaint(container) + " by " + StatColorHandler.StatPaint(value);
    }

    public override string DescriptionTextByStat(Stat stat)
    {
        var value = (percentage * 100).ToString() +"%" ;
        return "raises the " + StatColorHandler.StatPaint(stat.ToString().ToLower()) + " by " + StatColorHandler.StatPaint(value);
    }
}
