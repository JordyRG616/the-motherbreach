using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeEnemyManager : MonoBehaviour, IManager
{
    [SerializeField] [Range(0, 1)] private float thresholdToHeal;
    [SerializeField] private float ChildSpeed;
    [SerializeField] private List<GameObject> blueprints;
    [SerializeField] private int childrenCount;
    [SerializeField] private List<Transform> positionsToSpawn;
    [SerializeField] private float cooldown;
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
    private List<FormationManager> Children = new List<FormationManager>();
    [SerializeField] private Transform recallPosition;
    private bool recalling;
    public bool phaseTwoOn;
    

    void Start()
    {
        for(int i = 0; i < childrenCount; i++)
        {
            Invoke("SpawnChild", i);
        }
    }

    private void ManageChildren()
    {
        timer += Time.deltaTime;

        if(timer >= cooldown && !recalling)
        {
            if(Children.Count < childrenCount)
            {
                SpawnChild();
            } else if(phaseTwoOn)
            {
                var rdm = UnityEngine.Random.Range(0, Children.Count);
                var head = Children[rdm].transform.Find("Head");
                StartCoroutine(Recall(head));
            }
            timer = 0;
        }
    }

    private void SpawnChild()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        var rdm = UnityEngine.Random.Range(0, blueprints.Count);
        var child = Instantiate(blueprints[rdm], positionsToSpawn[index].position, Quaternion.identity);
        index++;
        Children.Add(child.GetComponent<FormationManager>());
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
        Children.Remove(sender as FormationManager);
    }

    void Update()
    {
        ManageChildren();
    }

    private IEnumerator Recall(Transform formationHead)
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;

        foreach(Rigidbody2D body in formationHead.parent.GetComponentsInChildren<Rigidbody2D>())
        {
            body.Sleep();
        }

        recalling = true;
        var position = formationHead.position;
        float step = 0;

        while(step <= 1)
        {
            if(formationHead == null) StopAllCoroutines();
            var newPos = Vector2.Lerp(position, recallPosition.position, step);
            formationHead.position = newPos;
            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.1f);

        Children.Remove(formationHead.GetComponentInParent<FormationManager>());
        Destroy(formationHead.parent.gameObject);

        yield return new WaitForSeconds(0.5f);
        step = 0;

        SpawnChild();
        SpawnChild();
        recalling = false;
        childrenCount += 1;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

    public void ReduceCooldown(float percentage)
    {
        cooldown -= cooldown * percentage;
    }

    public void ReceiveNewFormations(List<GameObject> newFormations)
    {
        foreach(GameObject formation in newFormations)
        {
            blueprints.Add(formation);
        }
    }

    public void DestroyManager()
    {
        foreach(FormationManager child in Children)
        {
            child.Terminate();
        }
    }
}
