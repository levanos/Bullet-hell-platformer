using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile_enemy : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public int damage = 40;
    public GameObject impactEffect;
    public Transform target;
    public float rotationSpeed;

    private Rigidbody2D rb;

    public GameObject destroyEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Heart").transform;
        Invoke("DestroyProjectile", lifeTime);
    }

    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotationSpeed;

        rb.velocity = transform.up * speed;
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
        if (other.gameObject.tag != "Bullet" && other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
