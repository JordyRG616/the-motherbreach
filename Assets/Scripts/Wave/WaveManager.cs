using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveList data;
    [SerializeField] private float distanceToSpawn;
    private ShipManager ship;
    private List<FormationManager> activeFormations = new List<FormationManager>();


    public void Start()
    {
        ship = ShipManager.Main;
        InstantiateFormation(PositionToSpawn());
    }

    private Vector3 PositionToSpawn()
    {
        float rdmAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(rdmAngle), Mathf.Sin(rdmAngle)); 
        return (distanceToSpawn * direction) + ship.transform.position;
    }

    private void InstantiateFormation(Vector3 position)
    {
        int rdm = Random.Range(0, data.waves[0].availableFormations.Count);
        var formationToSpawn = data.waves[0].availableFormations[rdm];

        GameObject formation = Instantiate(formationToSpawn, position, Quaternion.identity);
        var manager = formation.GetComponent<FormationManager>();
        manager.Initialize();
        activeFormations.Add(manager);
    }
}
