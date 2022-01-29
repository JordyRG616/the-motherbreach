using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor_SpecialState : BossState
{
    [SerializeField] private float sway;
    [SerializeField] private List<WeaponClass> weaponsOnEnter;
    [SerializeField] private List<WeaponClass> weaponsOnAction;
    [SerializeField] private float delayToSpreader;
    private float angle;


    public override void Action()
    {
        body.velocity = Vector2.zero;
        var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        body.AddForce(direction * sway);

        var _direction = (ship.position - transform.position).normalized;
        var _angle = (Mathf.Atan2(_direction.y, _direction.x) + Mathf.Sin(angle)/10) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _angle + 90f);// + 90f);

    }

    private void ActivateSpecial()
    {
        actionController.ActivateWeapons(weaponsOnAction);
    }

    public override void EnterState()
    {
        angle = 0;
        actionController.ActivateWeapons(weaponsOnEnter);
        Invoke("ActivateSpecial", delayToSpreader);
    }

    public override void ExitState()
    {
        actionController.StopWeapons();
    }
    
    void Update()
    {
        angle += Time.deltaTime;
    }
}
