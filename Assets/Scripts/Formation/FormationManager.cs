using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    [SerializeField] private List<RowData> rows;
    private FormationConstructor formationConstructor;
    private WiggleController wiggleController;
    private List<IManager> managers;


    public void Initialize()
    {
        formationConstructor = FormationConstructor.Main;
        wiggleController = GetComponent<WiggleController>();

        managers = GetComponents<IManager>().ToList();

        Fill();
    }

    public T GetManager<T>()
    {
        var manager = (T)managers.Find(x => x.GetType() == typeof(T));
        return manager;
    }

    private void Fill()
    {
        foreach(RowData row in rows)
        {
            formationConstructor.ConstructRow(row, this);
        }
    }
}

[System.Serializable]
public struct RowData
{
    public List<Vector2> enemyPositions;
    public EnemyType enemyType;
    public WigglePattern wigglePattern;
}