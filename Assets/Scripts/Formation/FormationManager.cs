using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationManager : MonoBehaviour
{
    public event EventHandler OnFormationDefeat;
    public List<EnemyManager> children {get; private set;} = new List<EnemyManager>();

    void Start()
    {
        children = GetComponentsInChildren<EnemyManager>().ToList();
    }

    void Update()
    {
        if(children.Count == 0)
        {
            OnFormationDefeat?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    public void RemoveEnemy(EnemyManager enemy)
    {
        children.Remove(enemy);
    }

}
