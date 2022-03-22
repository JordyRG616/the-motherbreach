using System.Collections;
using StringHandler;
using UnityEngine;

public class ViciousEffect : BaseEffectTemplate
{
    [SerializeField] [Range(0, 1)] private float buffPercentage;
    [SerializeField] private float durationOfBuff;
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] [FMODUnity.EventRef] private string sfx;
    [SerializeField] [ColorUsage(true, true)] private Color effectOnColor;
    [SerializeField] private ParticleSystem dissipateVFX;
    [SerializeField] [FMODUnity.EventRef] private string dissipateSfx;
    

    private bool buffOn;
    private float timer;
    private bool counting;

    public override void ApplyEffect()
    {
        if(!buffOn)
        {
            vfx.Play();
            AudioManager.Main.RequestSFX(sfx);
            associatedController.GetComponent<SpriteRenderer>().color = effectOnColor;
            // timer = 0;
            // counting = false;
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
            shooters[i].SetStat(Stat.Damage, ogValue * (1 + buffPercentage));
        }

        yield return new WaitForSeconds(durationOfBuff);

        for(int i = 0; i < shooters.Count; i++)
        {
            shooters[i].SetStat(Stat.Damage, ogValues[i]);
        }

        dissipateVFX.Play();
        AudioManager.Main.RequestSFX(dissipateSfx);
        associatedController.GetComponent<SpriteRenderer>().color = Color.white;
        buffOn = false;
    }
    
    public override string DescriptionText()
    {
        string description = "this turret gains " + StatColorHandler.StatPaint((buffPercentage * 100).ToString() + "%") + " extra " + StatColorHandler.DamagePaint("damage") + " for " + StatColorHandler.StatPaint(durationOfBuff.ToString()) + " seconds";
        return description;
    }

    // void FixedUpdate()
    // {
    //     timer += Time.fixedDeltaTime;
    // }
}
