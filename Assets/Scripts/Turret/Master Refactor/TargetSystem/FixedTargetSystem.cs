using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTargetSystem : TargetSystem
{
    private Collider2D fieldOfView;
    private List<Collider2D> contacts = new List<Collider2D>();

    private void Start()
    {
        fieldOfView = GetComponent<Collider2D>();
    }

    private void Update()
    {
        GetTarget();
    }

    private void GetTarget()
    {
        var count = fieldOfView.GetContacts(contacts);
        if (count == 0)
        {
            target = null;
            return;
        }



        var rdm = Random.Range(0, count);
        target = contacts[rdm].transform;
    }

}
