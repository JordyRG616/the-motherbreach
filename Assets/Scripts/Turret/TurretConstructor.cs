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
    [SerializeField] private RewardList baseList;
    [SerializeField] private RewardList topList;

    public void Initialize()
    {
        baseList.InitiateMatrix();
        topList.InitiateMatrix();
    }


    private GameObject GetBase(Transform parentBlueprint, RewardLevel level, ActionController controller)
    {
        GameObject container = Instantiate(GetPossibleBase(controller, level, out string baseName), transform.position, Quaternion.identity, parentBlueprint);
        container.name = baseName;
        container.transform.localPosition = Vector3.zero;
        return container;
    }

    private GameObject GetPossibleBase(ActionController controller, RewardLevel level, out string name)
    {
        List<GameObject> possibleBases = new List<GameObject>();
        List<WeaponClass> weapons = controller.GetClasses();

        foreach(GameObject reward in baseList.GetRewardsByLevel(level))
        {
            foreach(WeaponClass weapon in weapons)
            {
                if(reward.GetComponent<BaseEffectTemplate>().GetWeaponClasses().Contains(weapon))
                {
                    possibleBases.Add(reward);
                }
            }
        }

        int rdm = Random.Range(0, possibleBases.Count);

        name = possibleBases[rdm].name;

        return possibleBases[rdm];

    }

    private GameObject GetTop(Transform parentBlueprint, RewardLevel level, out ActionController actionController)
    {
        int rdm = Random.Range(0, topList.GetListCount(level));
        var _instance = topList.GetRewardByLevel(level, rdm);
        GameObject container = Instantiate(_instance, transform.position, Quaternion.identity, parentBlueprint);
        container.name = _instance.name;
        container.name = _instance.name;
        container.transform.localPosition = Vector3.zero;
        actionController = container.GetComponent<ActionController>();
        actionController.Initiate();
        return container;
    }

    public GameObject GetTop(RewardLevel level)
    {
        var list = topList.GetAllRewardsBelowLevel(level);
        int rdm = Random.Range(0, list.Count);
        var container = Instantiate(list[rdm]);
        container.name = list[rdm].name;
        container.GetComponent<ActionController>().Initiate();
        container.SetActive(false);
        //GameObject container = Instantiate(_instance, transform.position, Quaternion.identity);
        return container;
    }

    public GameObject GetBase(RewardLevel level)
    {
        var list = baseList.GetAllRewardsBelowLevel(level);
        int rdm = Random.Range(0, list.Count);
        var container = Instantiate(list[rdm]);
        container.name = list[rdm].name;
        container.SetActive(false);
        //GameObject container = Instantiate(_instance, transform.position, Quaternion.identity);
        return container;
    }

    public GameObject Construct(RewardLevel baseLevel, RewardLevel topLevel)
    {
        GameObject blueprint = Instantiate(TurretTemplate, transform.position, Quaternion.identity);

        GameObject _gun = GetTop(blueprint.transform, topLevel, out ActionController actionController);
        GameObject _base = GetBase(blueprint.transform, baseLevel, actionController);

        blueprint.GetComponent<TurretManager>().Initiate();

        TriggerImeddiateEffect(blueprint);


        return blueprint;
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

        if(baseEffect.GetTrigger() == BaseEffectTrigger.OnLevelUp) 
        {
            manager.OnLevelUp += baseEffect.HandleLevelEffect;
        }


        TriggerImeddiateEffect(blueprint);


        return blueprint;
    }
    
    private static void TriggerImeddiateEffect(GameObject occupyingTurret)
    {
        BaseEffectTemplate effect = occupyingTurret.GetComponentInChildren<BaseEffectTemplate>();
        effect.ReceiveWeapon(occupyingTurret.GetComponentInChildren<ActionController>());
        if (effect.GetTrigger() == BaseEffectTrigger.Immediate)
        {
            effect.ApplyEffect();
        }

    }
}
