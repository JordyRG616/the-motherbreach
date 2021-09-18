using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleController : MonoBehaviour, IManager
{
    private Dictionary<WigglePattern, List<EnemyWiggler>> wiggleMatrix = new Dictionary<WigglePattern, List<EnemyWiggler>>();

    [ContextMenu("teste")]
    public void test()
    {
        // foreach(List<EnemyWiggler> key in wiggleMatrix.Values)
        // {
        //     Debug.Log(key.Count);
        // }
        ActivateWigglers();
    }
    

    public void AddToMatrix(WigglePattern pattern, EnemyWiggler enemy)
    {
        if(wiggleMatrix.ContainsKey(pattern))
        {
            wiggleMatrix[pattern].Add(enemy);
        } else
        {
            List<EnemyWiggler> newWigglerList = new List<EnemyWiggler>();
            newWigglerList.Add(enemy);
            wiggleMatrix.Add(pattern, newWigglerList);
        }
    }

    public void DestroyManager()
    {
        foreach(WigglePattern pattern in wiggleMatrix.Keys)
        {
            foreach(EnemyWiggler enemy in wiggleMatrix[pattern])
            {
                enemy.StopWiggling();
            }
        }

        Destroy(this);
    }

    private void ActivateWigglers()
    {
        foreach(WigglePattern pattern in wiggleMatrix.Keys)
        {
            foreach(EnemyWiggler enemy in wiggleMatrix[pattern])
            {
                enemy.StartWiggling(pattern);
            }
        }
    }
}
