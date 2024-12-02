using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 50f;
    public GameObject hitEffectPrefab;
    public int damageAmount = 10; // Damage this bullet deals

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * bulletSpeed;
        }
        else
        {
            Debug.LogError("Rigidbody is missing from EnemyBullet prefab.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount); // Reduce player health
                Debug.Log($"EnemyBullet dealt {damageAmount} damage to the player!");
            }
            else
            {
                Debug.LogError("PlayerStats component missing on Player!");
            }

            CreateHitEffect(collision);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy bullet on other collisions
        }
    }


    void CreateHitEffect(Collision collision)
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity);
            Destroy(effect, 3f); // Destroy effect after 3 seconds
        }
    }
}
