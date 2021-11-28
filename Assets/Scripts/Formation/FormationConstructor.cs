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

    public void ConstructEnemies(List<EnemySlot> slots, FormationManager parentFormation)
    {
        foreach(EnemySlot slot in slots)
        {
            var instanceList = enemies.FindAll(x => x.GetEnemyType() == slot.enemyType);
            int rdm = Random.Range(0, instanceList.Count);
            var enemy = instanceList[rdm];

            if(availableEnemies.Contains(enemy.gameObject))
            {
                
                var container = Instantiate(enemy.gameObject, parentFormation.transform.position, Quaternion.identity, parentFormation.transform);
                container.transform.localPosition = slot.slotPosition;

                parentFormation.GetManager<WiggleController>().AddToMatrix(slot.wigglePattern, container.GetComponent<EnemyWiggler>());
                parentFormation.GetManager<FormationAttackController>().AddAttacker(container.GetComponent<EnemyAttackController>());
                parentFormation.GetManager<PopulationManager>().RegisterEnemy(container.GetComponent<EnemyHealthController>());
                
            }
        }  
    }


    
}
