using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEffect : ActionEffect
{
    [SerializeField] private GameObject droneTemplate;
    [SerializeField] private float capacity;
    [SerializeField] private float droneLevel;
    private List<GameObject> drones = new List<GameObject>();

    protected override void SetData()
    {
        StatSet.Add(Stat.Capacity, capacity);
        StatSet.Add(Stat.DroneLevel, droneLevel);

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
    }
    public override void ApplyEffect(HitManager hitManager)
    {
        
    }

    public override void Shoot()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while(drones.Count < capacity)
        {
            drones.Add(ConstructDrone());
        }
        
        int activeDrones = 0;

        while(activeDrones < capacity)
        {
            drones[activeDrones].GetComponent<DroneMovement>().StartMoving();
            drones[activeDrones].GetComponent<DroneController>().StartComponent();
            
            activeDrones ++;

            yield return new WaitForSecondsRealtime(1f);
        }
        
    }

    private GameObject ConstructDrone()
    {
        GameObject container = Instantiate(droneTemplate, Vector3.zero, Quaternion.identity, this.transform);
        container.GetComponent<DroneMovement>().Configure(FindTarget(), droneLevel);
        container.GetComponent<DroneController>().Configure(droneLevel);

        return container;
    }

    private Transform FindTarget()
    {
        var enemies = FindObjectsOfType<EnemyManager>();

        var enemy = enemies.OrderBy(x => Vector2.Distance(x.transform.position, transform.position)).FirstOrDefault();

        return enemy.transform;
    }
}
