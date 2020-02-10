using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blade : MonoBehaviour
{
    public GameObject impactEffect;
    public int damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        Patrol enemy = other.GetComponent<Patrol>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
    }
}
