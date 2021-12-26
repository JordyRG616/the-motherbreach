using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Reward/Reward List", fileName ="New Reward List")]
public class RewardList : ScriptableObject
{
    [SerializeField] private List<GameObject> Common;
    [SerializeField] private List<GameObject> Uncommon;
    [SerializeField] private List<GameObject> Rare;
    [SerializeField] private List<GameObject> Unique;

    private Dictionary<RewardLevel, List<GameObject>> listsByRewards = new Dictionary<RewardLevel, List<GameObject>>();

    public void InitiateMatrix()
    {
        listsByRewards.Add(RewardLevel.Common, Common);
        listsByRewards.Add(RewardLevel.Uncommon, Uncommon);
        listsByRewards.Add(RewardLevel.Rare, Rare);
        listsByRewards.Add(RewardLevel.Unique, Unique);
    }

    public int GetListCount(RewardLevel level)
    {
        return listsByRewards[level].Count;
    }

    public GameObject GetRewardByLevel(RewardLevel level, int index)
    {
        return listsByRewards[level][index];
    }

    public List<GameObject> GetRewardsByLevel(RewardLevel level)
    {
        return listsByRewards[level];
    }

}