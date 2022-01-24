using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    #region Singleton
    private static ShipManager _instance;
    public static ShipManager Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ShipManager>();
            }
            return _instance;
        }
    }
    #endregion

    private GameManager gameManager;
    private List<TurretManager> turrets = new List<TurretManager>();

    void Awake()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HealTurrets;
        gameManager.OnGameStateChange += HandleAnchor;
    }

    private void HandleAnchor(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward) GetComponent<MovableEntity>().Anchor();
        else GetComponent<MovableEntity>().LiftAnchor();

    }

    private void HealTurrets(object sender, GameStateEventArgs e)
    {
        if(e.newState == GameState.OnReward)
        {
            foreach(TurretManager turret in turrets)
            {
                turret.integrityManager.HealToFull();
            }
        }
    }

    public List<BaseEffectTemplate> GetBases()
    {
        List<BaseEffectTemplate> container = new List<BaseEffectTemplate>();

        foreach(TurretManager turret in turrets)
        {
            container.Add(turret.baseEffect);
        }

        return container;
    }

    public List<ActionController> GetWeapons()
    {
        List<ActionController> container = new List<ActionController>();

        foreach(TurretManager turret in turrets)
        {
            container.Add(turret.actionController);
        }

        return container;
    }

    public void RegisterTurret(TurretManager turret)
    {
        turrets.Add(turret);
    }

}
