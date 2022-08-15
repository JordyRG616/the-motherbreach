using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Wave List", fileName ="Wave List")]
public class WaveList : ScriptableObject
{
    [Header("Enemy Lists")]
    public List<WaveData> levelOneWaves;
    public List<WaveData> levelTwoWaves;
    public List<WaveData> levelThreeWaves;
    public List<WaveData> levelFourWaves;
    public Dictionary<int, List<WaveData>> listsByLevel = new Dictionary<int, List<WaveData>>();

    [Header("Boss Lists")]
    public List<WaveData> levelOneBosses;
    public Dictionary<int, List<WaveData>> bossesByLevel = new Dictionary<int, List<WaveData>>();

    [Header("Enemy Information")]
    public  List<EnemyInformation> enemyInformation;

    [Header("Boss Information")]
    public List<BossInformation> bossInformation;


    public void Initiate()
    {
        listsByLevel.Add(0, levelOneWaves);
        listsByLevel.Add(1, levelTwoWaves);
        listsByLevel.Add(2, levelThreeWaves);
        listsByLevel.Add(3, levelFourWaves);

        bossesByLevel.Add(0, levelOneBosses);
    }
}

[System.Serializable]
public struct EnemyInformation
{
    public Sprite enemySprite;
    public Sprite enemyOutline;
    [TextArea] public string enemyDescription;
}

[System.Serializable]
public struct BossInformation
{
    public Sprite bossSprite;
    public Sprite coreSprite;
    public string coreName;
    [TextArea] public string bossDescription;
}