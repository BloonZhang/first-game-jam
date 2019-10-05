using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    // Public variables
    public float healthValue;
    public float speed = 2;

    // Private variables
    Vector2 floatDirection;
    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        // Get a random direction
        floatDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        floatDirection.Normalize(); floatDirection = floatDirection * speed;

        // Get the RB2D
        rigidbody2d = GetComponent<Rigidbody2D>();
        // Send it off!
        rigidbody2d.AddForce(floatDirection);
    }

    // Update is called once per frame
    void Update()
    {
        // Constant rotate, very cool!
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.collider.GetComponent<PlayerController>();
        if (player != null) 
        {
            HealthBar.instance.changeValue(healthValue);
            PlayerController.instance.PlayCollectSound();
            Destroy(gameObject);
        }

    }

}
