using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipAbility : MonoBehaviour
{
    [SerializeField] protected float energyRechargeRate;
    [SerializeField] protected Sprite abilityIcon;
    protected float maxEnergy = 100;
    protected float currentEnergy = 0;
    
    protected ShipManager shipManager;
    protected Rigidbody2D body;
    protected ShipAbilityGUI GUI;



    protected virtual void Awake()
    {
        shipManager = GetComponent<ShipManager>();
        body = FindObjectOfType<ShipController>().GetComponent<Rigidbody2D>();
        GUI = FindObjectOfType<ShipAbilityGUI>();

        GUI.ReceiveIcon(abilityIcon);
    }
    
    protected abstract void ActionEffect();

    protected virtual void DoAction()
    {
        if(currentEnergy < maxEnergy) return;

        ActionEffect();

        currentEnergy = 0;
        GUI.SetFillPercentage(0);
    }

    protected virtual void GainEnergy()
    {
        if(currentEnergy >= maxEnergy) return;
        if(body.velocity.magnitude > .3f) return;
        if(GameManager.Main.gameState != GameState.OnWave) return;

        currentEnergy += energyRechargeRate;
        GUI.SetFillPercentage(currentEnergy/maxEnergy);
    }

    protected virtual void FixedUpdate()
    {
        GainEnergy();

        if(Input.GetMouseButtonDown(1))
        {
            DoAction();
        }
    }
}
