using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData data;
    private List<FormationManager> activeFormations = new List<FormationManager>();

    [ContextMenu("Test")]
    public void test()
    {
        int rdm = Random.Range(0, data.availableFormations.Count);
        var formationToSpawn = data.availableFormations[rdm];

        GameObject formation = Instantiate(formationToSpawn, transform.position, Quaternion.identity);
        var manager = formation.GetComponent<FormationManager>();
        manager.Initialize();
        activeFormations.Add(manager);
    }

}
