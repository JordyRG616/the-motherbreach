using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FormationConstructor : MonoBehaviour
{
    #region Singleton
    private static FormationConstructor _instance;
    public static FormationConstructor Main
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<FormationConstructor>();
                
                if(_instance == null)
                {
                    GameObject container = GameObject.Find("Game Manager");

                    if(container == null)
                    {
                        container = new GameObject("Game manager");
                    }
                    
                    _instance = container.AddComponent<FormationConstructor>();
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private List<GameObject> availableEnemies;
    private List<EnemyManager> enemies = new List<EnemyManager>();

    void Awake()
    {
        ConvertList();
    }
    

    private void ConvertList()
    {
        foreach(GameObject enemyObj in availableEnemies)
        {
            var container = enemyObj.GetComponent<EnemyManager>();
            enemies.Add(container);
        }
    }

    public void ConstructRow(RowData data, FormationManager parentFormation)
    {
        var instanceList = enemies.FindAll(x => x.GetEnemyType() == data.enemyType);
        int rdm = Random.Range(0, instanceList.Count);
        var enemy = instanceList[rdm];
        
        if(availableEnemies.Contains(enemy.gameObject))
        {
            foreach(Vector2 position in data.enemyPositions)
            {
                var container = Instantiate(enemy.gameObject, position, Quaternion.identity, parentFormation.transform);
                parentFormation.GetManager<WiggleController>().AddToMatrix(data.wigglePattern, container.GetComponent<EnemyWiggler>());
            }
        }
    }
}
