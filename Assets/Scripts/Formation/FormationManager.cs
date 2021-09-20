using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationManager : MonoBehaviour
{
    [SerializeField] private FormationSlotsData slotsData;
    private FormationConstructor formationConstructor;
    private List<IManager> managers;


    public void Initialize()
    {
        formationConstructor = FormationConstructor.Main;

        managers = GetComponents<IManager>().ToList();

        Fill();

        GetManager<WiggleController>().Initiate();
        GetManager<PopulationManager>().OnPopulationEmpty += DestroyFormation;
    }

    private void DestroyFormation(object sender, EventArgs e)
    {
        foreach(IManager manager in managers)
        {
            manager.DestroyManager();
        }

        Destroy(gameObject);
    }

    public T GetManager<T>()
    {
        var manager = (T)managers.Find(x => x.GetType() == typeof(T));
        return manager;
    }

    private void Fill()
    {
        formationConstructor.ConstructEnemies(slotsData.slots, this);
    }
    
}
