using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public int health = 100;
    public Transform groundDetection;
    public GameObject deathEffect;
    public GameObject projectile;
    public Transform shotPoint;
    public Transform orbPoint;
    public GameObject orb;

    private Animator anim;

    private bool movingRight = true;

    private void Start()
    {
        InvokeRepeating("Shoot", 0.5f, 0.5f);
        InvokeRepeating("ShootOrb", 5f, 5f);
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f);
        {
            if (groundInfo.collider == false)
            {
                if(movingRight == true)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Shoot()
    {
        Instantiate(projectile, shotPoint.position, shotPoint.rotation);
        if (movingRight)
        {
            Instantiate(projectile, shotPoint.position, Quaternion.Euler(0, 0, 110));
            Instantiate(projectile, shotPoint.position, Quaternion.Euler(0, 0, 70));
        }
        else
        {
            Instantiate(projectile, shotPoint.position, Quaternion.Euler(0, 180, 110));
            Instantiate(projectile, shotPoint.position, Quaternion.Euler(0, 180, 70));
        }
    }

    public void ShootOrb()
    {
        Instantiate(orb, orbPoint.position, orbPoint.rotation);
    }

}
