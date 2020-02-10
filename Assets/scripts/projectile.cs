using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage = 40;
    public GameObject impactEffect;

    public GameObject destroyEffect;

    private void Start()
    {
        Invoke("DestroyProjectile",lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(-transform.up * speed * Time.deltaTime, Space.World);
    }

    void DestroyProjectile()
    {
        //Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Patrol enemy = other.GetComponent<Patrol>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
        if(other.gameObject.tag != "Bullet")
            Destroy(gameObject);
    }
}
