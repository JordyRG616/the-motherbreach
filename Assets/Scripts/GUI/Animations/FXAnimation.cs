using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXAnimation : UIAnimations
{
    [SerializeField] private List<ParticleSystem> VfxList;
    [SerializeField] [FMODUnity.EventRef] private List<string> SfxList;

    public override bool Done { get; protected set; }

    public override IEnumerator Forward()
    {
        foreach(ParticleSystem vfx in VfxList)
        {
            vfx.Play();
        }

        foreach(string sfx in SfxList)
        {
            AudioManager.Main.RequestGUIFX(sfx);
        }

        yield return new WaitForEndOfFrame();

        Done = true;
    }

    public override IEnumerator Reverse()
    {
        throw new System.NotImplementedException();
    }
}
