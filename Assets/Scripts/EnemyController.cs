using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // Public variables
    public float moveSpeed = 0.5f;
    public float maxHealth = 10;
    public float delayBetweenShots = 2.0f;
    public bool hasWeapon;
    public GameObject bulletPrefab;
    public float enemyDamage;
    // ***** Look for death torque and death velocity

    // Properties
    //public float Health {get { return currentHealth; }}

    // Private Variables
    PlayerController player;
    float currentHealth;
    float shotTimer = 0;
    float stunTimer = 0;
    bool stunned = false;
    bool KOed = false;
    float KOTimer = 0;
    Vector3 aimDirection;

    // Sound
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {  
        // Audio source
        audioSource = GetComponent<AudioSource>();

        // Grab the player
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        if (playerArray == null){
            Debug.Log("Player array was null, weird.");
            return;
        }
        // Should be only one player
        player = playerArray[0].GetComponent<PlayerController>();

        // Set health
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        // Player is dead
        if (!PlayerController.instance.isAlive()){return;}
        // Alive code
        if (!KOed){
            
            // Stun timer count down
            if (stunned){
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0) {stunned = false;}
            }
            // Shot timer count down
            if (shotTimer > 0){ shotTimer -= Time.deltaTime; }

            // Fly and shoot towards the player if not stunned and has weapon
            if (!stunned && hasWeapon){
                // Aim
                aimDirection = player.transform.position - this.transform.position;
                float atan2 = Mathf.Atan2(aimDirection.y, aimDirection.x);
                transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                // Move
                Vector2 moveDirection = new Vector2(aimDirection.x, aimDirection.y); moveDirection.Normalize();
                Vector2 position = GetComponent<Rigidbody2D>().position;
                position = position + moveDirection * moveSpeed * Time.deltaTime;
                GetComponent<Rigidbody2D>().MovePosition(position);
                // Shoot
                if (shotTimer <= 0){
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    Projectile projectile = bullet.GetComponent<Projectile>();
                    projectile.Launch(new Vector2(aimDirection.x, aimDirection.y), enemyDamage);
                    shotTimer = delayBetweenShots;
                    audioSource.Play();
                } 
            } 
            // Spin really cute if no weapon
            else if (!stunned && !hasWeapon){
                if (!hasWeapon){
                    transform.Rotate(0, 0, 100 * Time.deltaTime);
                }
            }
        }
        // Dead code
        else
        {
            KOTimer += Time.deltaTime;
            if(KOTimer > 1.5f) { Explode(); }
        }
        
    }

    public void Stun(float stunTime)
    {
        stunned = true;
        stunTimer = stunTime;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        // KO if no health left
        if (currentHealth <= 0){
            Rigidbody2D rigidbody2d = GetComponent<Rigidbody2D>();
            rigidbody2d.constraints = RigidbodyConstraints2D.None;
            rigidbody2d.mass = 0.2f;
            rigidbody2d.drag = 3.0f;
            rigidbody2d.AddTorque(20); // ********** Death torque ********
            Vector2 knockbackDirection = new Vector2(-aimDirection.x, -aimDirection.y); knockbackDirection.Normalize();
            rigidbody2d.AddForce(knockbackDirection * 50); // ******** Death velocity ********
            KOed = true;
        }
    }

    
    void Explode()
    {
        Destroy(gameObject);
    }
        

}
