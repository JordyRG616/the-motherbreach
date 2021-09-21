using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCalculator : MonoBehaviour
{

    #region Singleton
    private static RewardCalculator _instance;
    public static RewardCalculator Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<RewardCalculator>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<RewardCalculator>();
                }
            }
            return _instance;
        }
    }
    #endregion
    
    Dictionary<float, RewardLevel> ranges = new Dictionary<float, RewardLevel>();

    [SerializeField] private RewardLevelData data;
    public float test;

    void Awake()
    {
        GenerateRanges();
        Debug.Log(ReturnRewardLevel(GammaDistribution(test)));
    }


    private void GenerateRanges()
    {   
        ranges.Clear();

        for (int i = 0; i < data.rewardLevels.Count; i++)
        {
            if(i == 0)
            {
                ranges.Add(RangeGammaDistribution(0, data.rewardLevels[i].maxProbabilityRange), data.rewardLevels[i].type);
            } else
            {
                ranges.Add(RangeGammaDistribution
                (
                    data.rewardLevels[i -1].maxProbabilityRange, 
                    data.rewardLevels[i].maxProbabilityRange), 
                    data.rewardLevels[i].type);
            }
        }

    }

    public RewardLevel ReturnRewardLevel(float x)
    {
        foreach(float range in ranges.Keys)
        {
            if(x <= range)
            {
                return ranges[range];
            }
        }
        return RewardLevel.Error;
    }


    private float GammaDistribution(float x)
    {
        float y = 1 - (Mathf.Exp(-x/2) * ((x/2) + 1));
        return y;
    }

    private float RangeGammaDistribution(float a, float b)
    {
        return GammaDistribution(b) - GammaDistribution(a);
    }  


}
