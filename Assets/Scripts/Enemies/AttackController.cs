using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private GameObject bulletSample;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private Transform target;
    [SerializeField] private float bulletSpeed;
    private int adjustIndex = -1;
    public int cooldown;

    void Awake()
    {
        if(target == null)
        {
            target = transform;
            adjustIndex = 1;
        }
    }

    internal void Attack()
    {
        Vector3 direction = (adjustIndex *(shootPosition.position - target.position)).normalized;
        GameObject bullet = Instantiate(bulletSample, shootPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
    }
    
}
