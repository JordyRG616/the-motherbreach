using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryTA : TopActionTemplate
{
    [SerializeField] private GameObject bulletSample;
    [SerializeField] private Transform shootPosition;
    [SerializeField]private float bulletSpeed;

    public override void ActivateAction(GameObject target)
    {
        Vector3 direction = target.transform.position - shootPosition.position;
        GameObject bullet = Instantiate(bulletSample, shootPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
    }
}
