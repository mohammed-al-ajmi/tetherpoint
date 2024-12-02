using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, IDamageable
{
    public int health = 5; // Health of the drone
    public float speed = 4f; // Slightly reduced movement speed
    public float hoverHeight = 10f; // Altitude to maintain above the player
    public float hoverOffset = 2f; // Offset to stay slightly ahead and to the side of the player
    public float attackCooldown = 2f; // Time between attacks
    public GameObject projectilePrefab;
    public Transform gunEnd; // Empty GameObject for shooting position
    public float projectileSpeed = 20f; // Speed of the projectile

    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        HoverNearPlayer();

        // Attack the player
        if (Time.time - lastAttackTime > attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    void HoverNearPlayer()
    {
        if (player == null) return;

        // Calculate the target position slightly ahead and to the side of the player
        Vector3 targetPosition = new Vector3(
            player.position.x + hoverOffset,  // Offset horizontally
            player.position.y + hoverHeight, // Maintain altitude
            player.position.z + hoverOffset  // Offset slightly forward
        );

        // Move toward the target position smoothly
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Look at the player while hovering
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void AttackPlayer()
    {
        if (projectilePrefab != null && gunEnd != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, gunEnd.position, gunEnd.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = (player.position - gunEnd.position).normalized * projectileSpeed;
            }

            // Ensure the projectile has the correct damage
            EnemyBullet bullet = projectile.GetComponent<EnemyBullet>();
            if (bullet != null)
            {
                bullet.damageAmount = 10; // Adjust damage value as needed
            }
        }
        else
        {
            Debug.LogError("ProjectilePrefab or GunEnd not assigned.");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Drone destroyed!");
        Destroy(gameObject);
    }
}
