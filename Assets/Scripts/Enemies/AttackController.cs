using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private GameObject bulletSample;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float bulletSpeed;
    public int cooldown;

    internal void Attack()
    {
        Vector3 direction = (shootPosition.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletSample, shootPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
    }
}
