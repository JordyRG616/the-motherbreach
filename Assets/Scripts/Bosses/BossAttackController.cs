using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    [SerializeField] private List<ActionEffect> weaponry;

    void Start()
    {
        foreach(ActionEffect weapon in weaponry)
        {
            weapon.Initiate();
        }
    }

    public void StopWeapons()
    {
        foreach(ActionEffect weapon in weaponry)
        {
            weapon.StopShooting();
        }
    }
}
