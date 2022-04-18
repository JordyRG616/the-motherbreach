using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAction : BossAction
{
    
    [SerializeField] private float ChildSpeed;
    [SerializeField] private List<GameObject> blueprints;
    [SerializeField] private List<Transform> positionsToSpawn;
    [SerializeField] private float cooldown;
    [Header("Effects")]
    [SerializeField] [FMODUnity.EventRef] private string spawnSFX;
    private ForgeController forgeController;
    private float _ogDuration;
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
        _ogDuration = actionDuration;
    }

    private void SpawnChild()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        var rdm = UnityEngine.Random.Range(0, blueprints.Count);
        var child = Instantiate(blueprints[rdm], positionsToSpawn[index].position, Quaternion.identity);

        positionsToSpawn[index].GetComponentInChildren<ParticleSystem>().Play();
        AudioManager.Main.RequestSFX(spawnSFX);

        index++;

        forgeController.Children.Add(child.GetComponent<FormationManager>());
        child.GetComponent<FormationManager>().OnFormationDefeat += removeChild;

        
        foreach(Rigidbody2D body in child.GetComponentsInChildren<Rigidbody2D>())
        {
            var _dir = body.position - (Vector2)transform.position;
            body.AddForce(_dir.normalized * ChildSpeed, ForceMode2D.Impulse);
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
        actionDuration = _ogDuration;
        timer = 0;
        if(forgeController.HasMaxCapacity())
        {
            actionDuration = 0.1f;
            return;
        } else
        {
            timer = cooldown;
        }
    }
}
