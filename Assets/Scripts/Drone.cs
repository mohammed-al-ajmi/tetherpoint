using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public int health = 5; // Health of the drone
    public float speed = 4f; // Slightly reduced movement speed
    public float hoverHeight = 10f; // Altitude to maintain above the player
    public float hoverOffset = 2f; // Offset to stay slightly ahead and to the side of the player
    public float attackCooldown = 2f; // Time between attacks
    public GameObject projectilePrefab;

    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        HoverNearPlayer();
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
        if (projectilePrefab != null && Time.time - lastAttackTime > attackCooldown)
        {
            // Create a projectile and shoot it toward the player
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = (player.position - transform.position).normalized * speed;
            lastAttackTime = Time.time;
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
