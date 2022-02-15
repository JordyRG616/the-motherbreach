using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretConstructor : MonoBehaviour
{   
    #region Singleton
    private static TurretConstructor _instance;
    public static TurretConstructor Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<TurretConstructor>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<TurretConstructor>();
                }
            }
            return _instance;
        }
    }
    #endregion


    [SerializeField] private GameObject TurretTemplate;
    private int lastRdmBase = int.MaxValue;
    private int lastRdmWeapon = int.MaxValue;
    private RewardCalculator rewardCalculator;


    public void Initialize()
    {
        rewardCalculator = RewardCalculator.Main;
    }

    public GameObject GetTop()
    {
        var list = rewardCalculator.weapons;
        int rdm = Random.Range(0, list.Count);
        if(rdm == lastRdmWeapon)
        {
            rdm = Random.Range(0, list.Count);
        }
        lastRdmWeapon = rdm;

        var container = Instantiate(list[rdm]);
        container.name = list[rdm].name;
        container.GetComponent<ActionController>().Initiate();
        container.SetActive(false);
        //GameObject container = Instantiate(_instance, transform.position, Quaternion.identity);
        return container;
    }

    public GameObject GetBase()
    {
        var list = rewardCalculator.bases;
        int rdm = Random.Range(0, list.Count);
        if(rdm == lastRdmBase)
        {
            rdm = Random.Range(0, list.Count);
        }
        lastRdmBase = rdm;

        var container = Instantiate(list[rdm]);
        container.name = list[rdm].name;
        container.SetActive(false);
        //GameObject container = Instantiate(_instance, transform.position, Quaternion.identity);
        return container;
    }

    public GameObject Construct(GameObject _weapon, GameObject _base)
    {
        GameObject blueprint = Instantiate(TurretTemplate, transform.position, Quaternion.identity);
        
        var manager = blueprint.GetComponent<TurretManager>();
        var baseEffect = _base.GetComponent<BaseEffectTemplate>();

        _weapon.SetActive(true);
        _weapon.transform.SetParent(blueprint.transform);
        _weapon.transform.localPosition = Vector3.zero;
        _base.SetActive(true);
        _base.transform.SetParent(blueprint.transform);
        _base.transform.localPosition = Vector3.zero;

        manager.Initiate();
        baseEffect.Initiate();

        // if(baseEffect.GetTrigger() == EffectTrigger.OnLevelUp) 
        // {
        //     Debug.Log("registered");
        //     manager.OnLevelUp += baseEffect.HandleLevelTrigger;
        // }

        return blueprint;
    }
    
    public void HandleBaseEffect(GameObject occupyingTurret)
    {
        BaseEffectTemplate effect = occupyingTurret.GetComponentInChildren<BaseEffectTemplate>();
        var weapon = occupyingTurret.GetComponentInChildren<ActionController>();
        effect.ReceiveWeapon(weapon);
        switch(effect.GetTrigger())
        {
            case EffectTrigger.Immediate:
                effect.ApplyEffect();
            break;
            case EffectTrigger.OnLevelUp:
                occupyingTurret.GetComponent<TurretManager>().OnLevelUp += effect.HandleLevelTrigger;
            break;
            case EffectTrigger.OnHit:
                occupyingTurret.GetComponent<HitManager>().OnHit += effect.HandleCommonTrigger;
            break;
            case EffectTrigger.OnDestruction:
                occupyingTurret.GetComponent<HitManager>().OnDeath += effect.HandleCommonTrigger;
            break;
            case EffectTrigger.OnTurretSell:
                FindObjectOfType<SellButton>(true).OnTurretSell += effect.HandleCommonTrigger;
            break;
            case EffectTrigger.OnTurretBuild:
                RewardManager.Main.OnTurretBuild += effect.HandleCommonTrigger;
            break;
        }
    }
}
