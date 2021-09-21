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

    void Start()
    {
        baseList.InitiateMatrix();
        topList.InitiateMatrix();
    }


    private GameObject GetBase(Transform parentBlueprint, RewardLevel level)
    {
        int rdm = Random.Range(0, baseList.GetListCount(level));
        GameObject container = Instantiate(baseList.GetRewardByLevel(level, rdm), transform.position, Quaternion.identity, parentBlueprint);
        container.transform.localPosition = Vector3.zero;
        return container;
    }

    private GameObject GetTop(Transform parentBlueprint, RewardLevel level)
    {
        int rdm = Random.Range(0, topList.GetListCount(level));
        GameObject container = Instantiate(topList.GetRewardByLevel(level, rdm), transform.position, Quaternion.identity, parentBlueprint);
        container.transform.localPosition = Vector3.zero;
        return container;
    }

    public GameObject Construct(RewardLevel baseLevel, RewardLevel topLevel)
    {
        GameObject blueprint = Instantiate(TurretTemplate, transform.position, Quaternion.identity);

        GameObject _gun = GetBase(blueprint.transform, baseLevel);
        GameObject _base = GetTop(blueprint.transform, topLevel);

        return blueprint;
    }
}
