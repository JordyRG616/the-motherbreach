using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Reward/Reward Level Data", fileName ="Reward Level Data")]
public class RewardLevelData : ScriptableObject
{
    public List<RewardType> rewardLevels;
}

[System.Serializable]
public struct RewardType
{
    public RewardLevel type;
    public float maxProbabilityRange;
}