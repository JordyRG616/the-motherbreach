using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeployerWeapon : Weapon
{
    [SerializeField] private Deployable deployableModel;
    [SerializeField] private Vector3 deployPoint;
    [SerializeField] private int deployQuantity = 1;
    [SerializeField] private float deployInterval;
    [HideInInspector] public float sizeMultiplier = 1;
    private Queue<Deployable> enqueuedDeployables = new Queue<Deployable>();
    private int activeDeployables;

    public override void Initiate()
    {
        base.Initiate();
        sizeMultiplier = 1;
        GetComponent<Capacity>().Initiate(shooter, this);
    }

    protected override void OpenFire()
    {
        for (int i = 0; i < deployQuantity; i++)
        {
            Invoke("Deploy", i * deployInterval);
        }

        attacking = true;
    }

    protected override void CeaseFire()
    {
        if(shooter != null) shooter.Stop(true);
        attacking = false;
        restCounter = 0;
    }

    protected override void SetInitialEffect()
    {
        totalEffect += ApplyDamage;
    }

    private void Deploy()
    {
        if (activeDeployables >= GetComponent<Capacity>().Value || gameManager.gameState != GameState.OnWave) return;
        _animator.SetTrigger("Deploy");
        Deployable container;
        if (enqueuedDeployables.Count > 0) 
        {
            container = enqueuedDeployables.Dequeue();
            container.gameObject.SetActive(true);
            container.transform.SetParent(transform);
        }
        else container = Instantiate(deployableModel, deployPoint, Quaternion.identity, transform);

        container.transform.localScale *= sizeMultiplier;
        container.Initialize();
        container.transform.localPosition = deployPoint;
        container.OnRedock += EnqueueDeployable;
        container.Launch();
        activeDeployables++;
        container.deployed = true;
    }

    private void EnqueueDeployable(Deployable deployable)
    {
        deployable.transform.localScale /= sizeMultiplier;
        deployable.deployed = false;
        deployable.OnRedock -= EnqueueDeployable;
        enqueuedDeployables.Enqueue(deployable);
        activeDeployables--;
    }

    protected override void Update()
    {
        if (activeDeployables >= GetComponent<Capacity>().Value) return;
        base.Update();
    }

    protected override IEnumerator ManageActivation()
    {
        while (true)
        {
            //_animator.SetBool("Attacking", false);

            yield return waitForCooldown;

            OpenFire();

            //yield return waitForDuration;
            yield return new WaitUntil(() => activeDeployables <= GetComponent<Capacity>().Value);

            CeaseFire();
        }
    }

    public void ReceiveDeployable(Deployable deployable)
    {
        enqueuedDeployables.Enqueue(deployable);
    }

    public void SetDeployPoint(Vector3 point)
    {
        deployPoint = point;
    }

    public void SetDeployQuantity(int value)
    {
        deployQuantity = value;
    }

    public Deployable GetModel()
    {
        return deployableModel;
    }

    public List<Deployable> GetInstantiatedDeployables()
    {
        return enqueuedDeployables.ToList();
    }
}
