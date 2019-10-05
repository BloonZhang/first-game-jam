using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Physics
    Rigidbody2D rigidbody2d;

    // Audio

    // Animations
    public ParticleSystem particles;

    // Public variables
    public float bulletSpeed = 300.0f; // Note that speed is affected by mass too
    public float bulletMass = 0.3f;
    //public float bulletDamage = 1.0f;
    public float bulletStun = 0.3f;
    public bool friendlyBullet;

    // Private variables
    float bulletDamage;


    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.mass = bulletMass;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Launch(Vector2 direction, float bulletDamage)
    {
        direction.Normalize();
        rigidbody2d.AddForce(direction * bulletSpeed);
        this.bulletDamage = bulletDamage;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        // Friendly code
        if (friendlyBullet){
            EnemyController enemy = other.collider.GetComponent<EnemyController>();
            if (enemy != null) {
                enemy.Stun(bulletStun);
                enemy.Damage(bulletDamage);
            }
        }
        // Enemy code
        else
        {
            PlayerController player = other.collider.GetComponent<PlayerController>();
            if (player != null) {
                HealthBar.instance.changeValue(-bulletDamage);
                PlayerController.instance.PlayHitSound();
            }
        }

        // If it hit the wall
        WallBoom wall = other.collider.GetComponent<WallBoom>();
        if (wall != null){ wall.PlayBoom(); }

        // Instantiate explode
        Instantiate(particles, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}

