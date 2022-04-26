using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployAction : BossAction
{
    [SerializeField] private GameObject model;
    [SerializeField] private float interval;
    [SerializeField] private float deployForce;
    [SerializeField] private float deployableDuration;
    private int sign = 1;

    private void AddForce(GameObject deployedModel)
    {
        var body = deployedModel.GetComponent<Rigidbody2D>();

        var direction = ship.position - transform.position;
        direction += (Vector3)Vector2.Perpendicular(direction) * sign;
        sign *= -1;

        body.AddForce(direction.normalized * deployForce, ForceMode2D.Impulse);

    }

    private void CreateModel()
    {
        var _obj = Instantiate(model, transform.position, Quaternion.identity);
        _obj.GetComponent<ActionEffect>().Initiate();
        _obj.GetComponentInChildren<ParticleSystem>().Play();
        AddForce(_obj);
    }

    public override void Action()
    {

    }

    public override void DoActionMove()
    {
        
    }

    public override void EndAction()
    {
    }

    public override void StartAction()
    {
        CreateModel();
        Invoke("CreateModel", interval);
    }
}
