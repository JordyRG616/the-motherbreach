using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployablePool : MonoBehaviour
{
    [SerializeField] private DeployableObject model;
    [SerializeField] private int estimateCount;

    private Queue<DeployableObject> freeDeployables = new Queue<DeployableObject>();
    private List<DeployableObject> deployed = new List<DeployableObject>();

    public void Initiate()
    {
        for (int i = 0; i < estimateCount; i++)
        {
            CreateDeployable();
        }
    }

    private void CreateDeployable()
    {
        var _obj = Instantiate(model, transform.position, Quaternion.identity);
        _obj.onLifetimeEnd += ReturnToFreeQueue;
        _obj.gameObject.SetActive(false);
        freeDeployables.Enqueue(_obj);
    }

    private void ReturnToFreeQueue(object sender, EventArgs e)
    {
        var deployedObject = (DeployableObject)sender;

        deployed.Remove(deployedObject);
        freeDeployables.Enqueue(deployedObject);
    }

    public DeployableObject RequestDeployable()
    {
        if(freeDeployables.Count == 0)
        {
            CreateDeployable();
        }

        var container = freeDeployables.Dequeue();
        deployed.Add(container);

        return container;
    }

    public int GetDeployedObjectCount()
    {
        return deployed.Count;
    }
}
