using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecruiserDeployable : MonoBehaviour, IDamageable
{
    [SerializeField] private ActionEffect retaliationWeapon;
    [SerializeField] private float speed;
    [SerializeField] private int maxHealth;
    private float currentHealth;

    private Transform ship;

    public void Initiate(Transform parent)
    {
        currentHealth = maxHealth;
        ship = parent;
        retaliationWeapon.Initiate();
        retaliationWeapon.ReceiveTarget(ShipManager.Main.gameObject);

        StartCoroutine(Orbit());
    }

    public void DestroyDamageable()
    {
        Destroy(gameObject);
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        if(currentHealth <= 0)
        {
            DestroyDamageable();
        }
        
        retaliationWeapon.Shoot();
        
    }

    public void UpdateHealthBar()
    {
    }

    private IEnumerator Orbit()
    {
        while(true)
        {
            var direction = transform.position - ship.position;

            var angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);

            direction += (Vector3)Vector2.Perpendicular(direction) * speed;

            transform.position = ship.position + (direction.normalized * 7);

            yield return new WaitForSeconds(0.01f);
        }

    }
}
