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
    [SerializeField] private List<GameObject> allWeapons;
    [SerializeField] private List<GameObject> allBases;
    [SerializeField] private List<Program> allPrograms;
    [SerializeField] private List<Program> unlockedPrograms;

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
        container.SetActive(false);
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
        return container;
    }

    public GameObject Construct(GameObject _weapon, GameObject _base)
    {
        GameObject blueprint = Instantiate(TurretTemplate, transform.position, Quaternion.identity);
        
        var manager = blueprint.GetComponent<TurretManager>();

        _weapon.SetActive(true);
        _weapon.transform.SetParent(blueprint.transform);
        _weapon.transform.localPosition = Vector3.zero;
        _base.SetActive(true);
        _base.transform.SetParent(blueprint.transform);
        _base.transform.localPosition = Vector3.zero;

        manager.Initiate();
      
        return blueprint;
    }

    public GameObject GetWeaponById(int weaponID)
    {
        var weapon = allWeapons.Find(x => x.GetComponent<Weapon>().Id == weaponID);
        var weaponInstance = Instantiate(weapon, Vector3.zero, Quaternion.identity);
        weaponInstance.name = weapon.name;
        weaponInstance.SetActive(false);
        return weaponInstance;
    }

    public GameObject GetWeaponPrefabById(int weaponID)
    {
        var weapon = allWeapons.Find(x => x.GetComponent<Weapon>().Id == weaponID);
        return weapon;
    }

    public GameObject GetBaseById(int baseID)
    {
        var _base = allBases.Find(x => x.GetComponent<Foundation>().Id == baseID);
        var baseInstance = Instantiate(_base, Vector3.zero, Quaternion.identity);
        baseInstance.name = _base.name;
        baseInstance.SetActive(false);
        return baseInstance;
    }

    public GameObject GetBasePrefabById(int baseID)
    {
        var _base = allBases.Find(x => x.GetComponent<Foundation>().Id == baseID);
        return _base;
    }

    public Program GetProgramById(int programID)
    {
        var _program = allPrograms.Find(x => x.Id == programID);
        return _program;
    }

    public Program GetRandomUnlockedProgram()
    {
        var rdm = Random.Range(0, unlockedPrograms.Count);
        return unlockedPrograms[rdm];
    }

    public void AddUnlockedProgram(Program program)
    {
        if (unlockedPrograms.Contains(program)) return;
        unlockedPrograms.Add(program);
    }
}
