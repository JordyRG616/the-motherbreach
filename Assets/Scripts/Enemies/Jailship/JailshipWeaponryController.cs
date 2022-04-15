using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailshipWeaponryController : MonoBehaviour
{
    [SerializeField] private ActionEffect weapon;
    [SerializeField] private float meanActivationTime;
    [SerializeField] private float activationTimeDeviation;
    public bool fixedRotation;
    private WaitForSeconds activationTime;
    private Transform ship;


    void Start()
    {
        ship = ShipManager.Main.transform;

        var rdm = Random.Range(-activationTimeDeviation, activationTimeDeviation);
        activationTime = new WaitForSeconds(meanActivationTime + rdm);

        StartCoroutine(ManageActivation());
    }

    private IEnumerator ManageActivation()
    {
        while(true)
        {
            yield return activationTime;
            
            weapon.Shoot();
        }
    }

    private void LookAtShip()
    {
        var direction = ship.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void Update()
    {
        if(fixedRotation) return;
        LookAtShip();
    }

    public void CeaseFire()
    {
        weapon.GetShooterSystem().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
