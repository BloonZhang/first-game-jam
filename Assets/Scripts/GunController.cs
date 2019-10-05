using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{

    // The player the gun is attached to
    public GameObject player;
    // The main camera
    public Camera mainCamera;

    // Audio
    public AudioClip shootClip;
    AudioSource audioSource;

    // Public variables
    public float fireDelay = 0.15f;
    public float ammoValue = 3.0f;
    public float bulletDamage = 1.0f;
    public GameObject bulletPrefab;
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI FireRateText;

    // Private variables
    Vector3 mouseDirection;
    float fireReloadTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Register input direction
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(player.transform.position);
        mouseDirection = Input.mousePosition - new Vector3(screenPosition.x, screenPosition.y, 0);


        // If the cooldown isn't back yet
        if (fireReloadTime > 0){
            fireReloadTime -= Time.deltaTime;
            return;
        } 
    }

    void FixedUpdate()
    {
        // Change direction
        float atan2 = Mathf.Atan2(mouseDirection.y, mouseDirection.x);
        //transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);

        // Change position
        Vector2 currentPosition = transform.position;
        Vector2 playerPosition; playerPosition.x = player.transform.position.x; playerPosition.y = player.transform.position.y;
        currentPosition.x = mouseDirection.x / 150.0f;
        currentPosition.y = mouseDirection.y / 150.0f;
        currentPosition = Vector2.ClampMagnitude(currentPosition, 2);
        transform.SetPositionAndRotation(playerPosition + currentPosition, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg));
    }

    public void Launch()
    {
        // If the cooldown is good
        if (fireReloadTime <= 0)
        {
            // Cost ammo
            HealthBar.instance.changeValue(-ammoValue);

            // Get player position
            Vector3 playerPosition = player.transform.position;

            fireReloadTime = fireDelay;
            float atan2 = Mathf.Atan2(mouseDirection.y, mouseDirection.x);
            GameObject bullet = Instantiate(bulletPrefab, playerPosition, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg));
            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.Launch(mouseDirection, bulletDamage);

            audioSource.PlayOneShot(shootClip);
        }
    
    }

    public void upgradeDamage()
    {
        bool enoughHealth = HealthBar.instance.buyUpgrade();
        if (enoughHealth){bulletDamage += 0.5f;}
        DamageText.text = "Current: " + System.Math.Round(bulletDamage, 2);
    }

    public void downgradeDamage()
    {
        bool enoughHealth = HealthBar.instance.buyDowngrade();
        if (enoughHealth){
            bulletDamage -= 0.5f;
            if (bulletDamage <= 0.1f) {bulletDamage = 0.1f;}
        }
        DamageText.text = "Current: " + System.Math.Round(bulletDamage, 2);
    }

    public void upgradeFireRate()
    {
        bool enoughHealth = HealthBar.instance.buyUpgrade();
        if (enoughHealth){
            fireDelay -= 0.03f;
            if (fireDelay <= 0.01f) {fireDelay = 0.01f;}
        }
        FireRateText.text = "Current: " + System.Math.Round(fireDelay, 2);
    }

    public void downgradeFireRate()
    {
        bool enoughHealth = HealthBar.instance.buyDowngrade();
        if (enoughHealth){
            fireDelay += 0.03f;
        }
        FireRateText.text = "Current: " + System.Math.Round(fireDelay, 2);
    }

}
