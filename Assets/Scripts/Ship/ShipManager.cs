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
    [SerializeField] private Transform controller;

    void Awake()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HealTurrets;
        // gameManager.OnGameStateChange += HandleAnchor;
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

    public void RegisterTurret(TurretManager turret)
    {
        turrets.Add(turret);
    }

    internal void RemoveTurret(TurretManager turretManager)
    {
        turrets.Remove(turretManager);
    }
    
    void Update()
    {
        transform.position = controller.position;
        transform.rotation = controller.rotation;
    }
}
