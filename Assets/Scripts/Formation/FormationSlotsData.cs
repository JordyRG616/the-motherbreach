using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Formation Slots", fileName ="New Formation Slots")]
public class FormationSlotsData : ScriptableObject
{

    public List<EnemySlot> slots;
}

    [System.Serializable]
    public struct EnemySlot
    {
    public Vector2 slotPosition;
    public EnemyType enemyType;
    public WigglePattern wigglePattern;
    }