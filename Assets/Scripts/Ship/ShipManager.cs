using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour, ISavable
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

    public int index;
    [HideInInspector] public int pilotIndex;
    private GameManager gameManager;
    public List<TurretManager> turrets {get; private set;}= new List<TurretManager>();
    private Transform controller;
    private Camera cam;
    public List<Artifact> artifacts {get; private set;} = new List<Artifact>();
    private ArtifactsPanel artifactsPanel;

    private List<ShipSubroutine> subroutines = new List<ShipSubroutine>();
    private Dictionary<string, List<Trait>> updatingFoundations = new Dictionary<string, List<Trait>>();

    public delegate void UpdatingFoundationEvent(string key, int count);
    public UpdatingFoundationEvent OnTraitCountUpdate;

    void Awake()
    {
        gameManager = GameManager.Main;
        gameManager.OnGameStateChange += HealTurrets;

        artifactsPanel = FindObjectOfType<ArtifactsPanel>();

        cam = Camera.main;

        controller = FindObjectOfType<ShipController>().transform;

        if (gameManager.selectedMap == Map.Tutorial) CreateWeaponry();
    }

    private void CreateWeaponry()
    {
        var slots = FindObjectsOfType<TurretSlot>().ToList();
        var turretConstructor = TurretConstructor.Main;

        for (int i = 0; i < 2; i++)
        {
            var weapon = turretConstructor.GetWeaponById(1);
            var foundation = turretConstructor.GetBaseById(1);
            var rdm = CustomRandom.RandomManager.GetRandomInteger(0, slots.Count);
            var turret = turretConstructor.Construct(weapon, foundation);
            slots[rdm].BuildTurret(turret);
            slots.RemoveAt(rdm);
            RegisterTurret(turret.GetComponent<TurretManager>());
        }
    }

    private IEnumerator WatchTurretCount()
    {
        yield return new WaitUntil(() => turrets.Count == 0);

        GetComponent<ShipDamageController>().DieByLackOfTurrets();
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
            StopCoroutine(WatchTurretCount());
            foreach(TurretManager turret in turrets)
            {
                turret.integrityManager.HealToFull();
            }
        }
        if(e.newState == GameState.OnWave)
        {
            StartCoroutine(WatchTurretCount());
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

    public void ReceiveSubroutine(ShipSubroutine subroutine)
    {
        subroutines.Add(subroutine);
        subroutines.ForEach(x => x.UpdateSubroutine());
    }

    public void ReceiveUpdatingTrait(string key, Trait trait)
    {
        int count = 0;

        if (updatingFoundations.ContainsKey(key))
        {
            var list = updatingFoundations[key];

            list.Add(trait);
            count = list.Count;

        } else
        {
            var list = new List<Trait>();

            list.Add(trait);
            updatingFoundations.Add(key, list);
            count = list.Count;
        }


        OnTraitCountUpdate?.Invoke(key, count);
    }

    public void RemoveUpdatingTrait(string key, Trait trait)
    {
        int count = 0;

        if(updatingFoundations.ContainsKey(key))
        {
            var list = updatingFoundations[key];

            list.Remove(trait);
            count = list.Count;
            OnTraitCountUpdate?.Invoke(key, count); 
        }
    }

    public Dictionary<string, byte[]> GetData()
    {
        var container = new Dictionary<string, byte[]>();

        container.Add("ShipHealth", BitConverter.GetBytes(GetComponent<ShipDamageController>().GetMissingHealth()));
        container.Add("ShipIndex", BitConverter.GetBytes(index));
        container.Add("PilotIndex", BitConverter.GetBytes(pilotIndex));

        foreach(TurretManager turret in turrets)
        {
            var data = turret.GetData();

            foreach(string key in data.Keys)
            {
                container.Add(key, data[key]);
            }
        }

        return container;
    }

    public void LoadData(SaveFile saveFile)
    {
        var health = BitConverter.ToSingle(saveFile.GetValue("ShipHealth"));
        GetComponent<ShipDamageController>().UpdateHealthNoEffects(-health);

        var slots = FindObjectsOfType<TurretSlot>();

        foreach(TurretSlot slot in slots)
        {
            if(saveFile.ContainsSavedContent(slot.slotID + "weaponLevel"))
            {
                var _w = TurretConstructor.Main.GetWeaponById(BitConverter.ToInt32(saveFile.GetValue(slot.slotID + "weaponID")));
                var _b = TurretConstructor.Main.GetBaseById(BitConverter.ToInt32(saveFile.GetValue(slot.slotID + "Foundation")));
                var loadedTurret = TurretConstructor.Main.Construct(_w, _b);

                slot.BuildTurret(loadedTurret);

                //TurretConstructor.Main.HandleBaseEffect(loadedTurret);

                RegisterTurret(loadedTurret.GetComponent<TurretManager>());

                loadedTurret.GetComponent<TurretManager>().LoadData(saveFile);
            }
        }
    }
}
