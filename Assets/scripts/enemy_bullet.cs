using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_bullet: MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage = 40;
    public GameObject impactEffect;

    public GameObject destroyEffect;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
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
        if (other.tag == "Heart")
        {
            charmovement player = other.GetComponentInParent<charmovement>();
            if (player != null)
            {
                Debug.Log("hit");
                player.TakeDamage(damage);
                //Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.tag != "Bullet" && other.gameObject.tag != "Enemy" && other.gameObject.tag != "Blade" && other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
