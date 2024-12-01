using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 50f;
    public GameObject hitEffectPrefab;

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
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Reduce player health by 10
            }

            CreateHitEffect(collision);
            Destroy(gameObject);
        }
        else
        {
            CreateHitEffect(collision);
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
