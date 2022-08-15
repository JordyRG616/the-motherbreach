using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Unlockables/Energy Overload", fileName = "Energy Overload")]
public class EnergyOverload : Trait
{
    public float percentage;
    public float duration;

    private GameManager gameManager;
    private Weapon linkedWeapon;
    private WaitForSeconds waitTime;

    public override void ApplyEffect(Weapon weapon)
    {
        linkedWeapon = weapon;
        waitTime = new WaitForSeconds(duration);

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange +=
            (object sender, GameStateEventArgs e) =>
                {
                    if(e.newState == GameState.OnWave) weapon.StartCoroutine(ManageEffect());
                };

    }

    private IEnumerator ManageEffect()
    {
        if(linkedWeapon.HasStat<Damage>(out var damage))
        {
            damage.ApplyPercentage(percentage);

            yield return waitTime;

            damage.RemovePercentage(percentage);
        }
    }

    public override string Description()
    {
        return "Raise the damage of this turret by " + percentage * 100 + "% in the first " + duration + " seconds of the wave";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (EnergyOverload)ScriptableObject.CreateInstance(type);

        instance.percentage = percentage;
        instance.duration = duration;

        return instance;
    }
}
