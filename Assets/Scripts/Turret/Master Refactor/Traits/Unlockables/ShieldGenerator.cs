using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Traits/Unlockables/Shield Generator", fileName = "Shield Generator")]
public class ShieldGenerator : Trait
{
    public GameObject shield;
    private GameManager gameManager;
    private WaveManager waveManager;
    private Weapon linkedWeapon;
    private GameObject instantiatedShield;

    public override void ApplyEffect(Weapon weapon)
    {
        linkedWeapon = weapon;

        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += CreateShield;

        waveManager = WaveManager.Main;
        waveManager.OnWaveEnd += DestroyShield;

        weapon.GetComponentInParent<TurretManager>().OnTurretDestruction += Destroy;
    }

    private void CreateShield(object sender, GameStateEventArgs e)
    {
        if (e.newState != GameState.OnWave) return;

        instantiatedShield = Instantiate(shield, Vector3.zero, Quaternion.identity, linkedWeapon.transform);
        instantiatedShield.transform.localPosition = Vector3.zero;
    }

    private void DestroyShield(object sender, EndWaveEventArgs e)
    {
        if (instantiatedShield == null) return;

        instantiatedShield.GetComponent<ShieldManager>().DestroyShield();
    }

    private void Destroy()
    {
        gameManager.OnGameStateChange -= CreateShield;
        waveManager.OnWaveEnd -= DestroyShield;
    }

    public override string Description()
    {
        return "Deploys a shield at the start of the wave";
    }

    public override Trait ReturnTraitInstance()
    {
        var type = this.GetType();
        var instance = (ShieldGenerator)ScriptableObject.CreateInstance(type);

        instance.shield = shield;

        return instance;
    }
}
