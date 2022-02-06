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

    public void ActivateWeapons(List<WeaponClass> classesToActivate)
    {
        foreach(ActionEffect weapon in weaponry)
        {
            if(classesToActivate.Contains(weapon.GetClass()))
            {
                weapon.ReceiveTarget(ShipManager.Main.gameObject);
                weapon.Shoot();
            } 
        }
    }

    public void StopWeapons()
    {
        foreach(ActionEffect weapon in weaponry)
        {
            weapon.StopShooting();
        }
    }

    public List<ActionEffect> RetrieveWeapons(WeaponClass classToRetrive)
    {
        var container = weaponry.FindAll(x => x.GetClass() == classToRetrive).ToList();
        return container;
    }
}
