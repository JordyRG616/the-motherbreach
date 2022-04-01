using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Artifact
{
    [SerializeField] private GameObject shield;

    public override string Description()
    {
        return "deploys a shield to a random turret at the start of each wave";
    }

    protected override void Effect()
    {
        var turrets = ShipManager.Main.turrets;
        var rdm = Random.Range(0, turrets.Count);

        Instantiate(shield, Vector3.zero, Quaternion.identity, turrets[rdm].transform).transform.localPosition = Vector3.zero;

    }

    
}
