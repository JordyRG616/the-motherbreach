using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAction : BossAction
{
    
    [SerializeField] private float ChildSpeed;
    [SerializeField] private List<GameObject> blueprints;
    [SerializeField] private int childrenCount;
    [SerializeField] private List<Transform> positionsToSpawn;
    [SerializeField] private float cooldown;
    private ForgeController forgeController;
    private float timer;
    private int _index;
    private int index
    {
        get
        {
            return _index;
        }

        set
        {
            if(value < positionsToSpawn.Count) _index = value;
            else _index = 0;
        }
    }

    void Start()
    {
        forgeController = GetComponent<ForgeController>();
    }

    private void SpawnChild()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        var rdm = UnityEngine.Random.Range(0, blueprints.Count);
        var child = Instantiate(blueprints[rdm], positionsToSpawn[index].position, Quaternion.identity);
        positionsToSpawn[index].GetComponentInChildren<ParticleSystem>().Play();
        index++;
        forgeController.Children.Add(child.GetComponent<FormationManager>());
        child.GetComponent<FormationManager>().OnFormationDefeat += removeChild;
        var angle = UnityEngine.Random.Range(0, 360);
        var rdmVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        foreach(Rigidbody2D body in child.GetComponentsInChildren<Rigidbody2D>())
        {
            body.AddForce(rdmVector * ChildSpeed, ForceMode2D.Impulse);
        }
    }

    private void removeChild(object sender, EventArgs e)
    {
        forgeController.Children.Remove(sender as FormationManager);
    }

    public void DestroyManager()
    {
        foreach(FormationManager child in forgeController.Children)
        {
            child.Terminate();
        }
    }

    public override void Action()
    {
        timer += Time.deltaTime;

        if(timer <= cooldown) return;
        SpawnChild();
        timer = 0;
    }

    public override void DoActionMove()
    {

    }

    public override void EndAction()
    {

    }

    public override void StartAction()
    {
        timer = cooldown;
    }
}
