using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    [SerializeField] private FormationSlotsData slotsData;
    private FormationConstructor formationConstructor;
    private WiggleController wiggleController;
    private List<IManager> managers;


    public void Initialize()
    {
        formationConstructor = FormationConstructor.Main;
        wiggleController = GetComponent<WiggleController>();

        managers = GetComponents<IManager>().ToList();

        Fill();

        wiggleController.Initiate();
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
