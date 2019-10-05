using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Singleton
    public static PlayerController instance {get; private set;}

    // Audio
    public AudioClip collectClip;
    public AudioClip explodeClip;
    public AudioClip hitClip;
    AudioSource audioSource;

    // Public variables
    public float moveSpeed;
    public float invincibleTime = 1.0f;
    //public float maxStat = 350;
    public GunController weapon;
    public ParticleSystem DamageExplosion;
    public TextMeshProUGUI SpeedText;

    // Private variables
    float invincibleTimer = 0.0f;
    bool alive = true;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive()){ return; }

        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
        Vector2 position = GetComponent<Rigidbody2D>().position;
        position = position + move * moveSpeed * Time.deltaTime;
        GetComponent<Rigidbody2D>().MovePosition(position);

        if (Input.GetMouseButton(0))
        {
            weapon.Launch();
        }

        // Reduce invincible timer
        if (invincibleTimer > 0.0f){
            invincibleTimer -= Time.deltaTime;
        }

    }

    // Damage if hit enemy
    void OnCollisionEnter2D(Collision2D other){
        EnemyController enemy = other.collider.GetComponent<EnemyController>();
        if (enemy != null && invincibleTimer <= 0.0f)
        {
            Instantiate(DamageExplosion, other.GetContact(0).point, Quaternion.identity);
            HealthBar.instance.changeValue(-25);
            invincibleTimer = invincibleTime;
            PlayHitSound();
        }
    }

    public void PlayHitSound(){
        audioSource.PlayOneShot(hitClip);
    }

    public void PlayCollectSound(){
        audioSource.PlayOneShot(collectClip);
    }

    public Vector2 GetPlayerPosition(){
        return transform.position;
    }

    public void upgradeSpeed(){
        bool enoughHealth = HealthBar.instance.buyUpgrade();
        if(enoughHealth) {moveSpeed += 1.5f;}
        SpeedText.text = "Current: " + System.Math.Round(moveSpeed, 2);
    }

    public void downgradeSpeed(){
        bool enoughHealth = HealthBar.instance.buyDowngrade();
        if(enoughHealth){
            moveSpeed -= 1.5f;
            if (moveSpeed <= 0.1f) {moveSpeed = 0.1f;}
        }
        SpeedText.text = "Current: " + System.Math.Round(moveSpeed, 2);
    }

    public void GameOver(){
        audioSource.PlayOneShot(explodeClip);
        Instantiate(DamageExplosion, transform.position, Quaternion.identity);
        Instantiate(DamageExplosion, transform.position, Quaternion.identity);
        Instantiate(DamageExplosion, transform.position, Quaternion.identity);
        ShopManager.instance.EndGame();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        alive = false;
    }

    public bool isAlive(){ return alive; }
}
