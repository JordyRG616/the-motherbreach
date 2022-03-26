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
    public List<TurretManager> turrets {get; private set;}= new List<TurretManager>();
    [SerializeField] private Transform controller;
    private Camera cam;
    [SerializeField] private float beamSelfDamage;
    public List<Artifact> artifacts {get; private set;} = new List<Artifact>();
    private ArtifactsPanel artifactsPanel;

    void Awake()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HealTurrets;

        artifactsPanel = FindObjectOfType<ArtifactsPanel>();

        cam = Camera.main;

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

    public int GetTurretCount()
    {
        return turrets.Count;
    }

    public void ReceiveArtifact(Artifact artifact)
    {
        artifact.Initialize();
        artifacts.Add(artifact);
        artifactsPanel.CreateNewBox(artifact);
    }
}
