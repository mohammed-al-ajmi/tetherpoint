using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private GameObject m_DroneBlades;
    private Light m_DroneLight;
    private float lastAttackTime;

    private bool m_IsAlive = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        m_DroneBlades = GameObject.Find("DroneBlades");
        m_DroneLight = GameObject.Find("DroneLight").GetComponent<Light>();
    }

    void Update()
    {
        if (!m_IsAlive) {
            return;
        }

        if (IsInRange()) {
            m_DroneLight.color = Color.red;
            HoverNearPlayer();
        }
        else {
            m_DroneLight.color = Color.yellow;
        }

        // Attack the player
        if (Time.time - lastAttackTime > attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }

        // m_DroneBlades.transform.localEulerAngles += new Vector3(0.0f, 0.0f, 100.0f * Time.deltaTime);

        m_DroneBlades.transform.Rotate(0.0f, 0.0f, 1700.0f * Time.deltaTime);
    }

    bool IsInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= 30.0f;
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
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f * Time.deltaTime);

        // Look at the player while hovering
        // transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    

    void AttackPlayer()
    {
        if (projectilePrefab != null && gunEnd != null)
        {
            // Create a projectile and shoot it toward the player
            GameObject projectile = Instantiate(projectilePrefab, gunEnd.position, gunEnd.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = (player.position - gunEnd.position).normalized * projectileSpeed;
            }
        }
        else
        {
            Debug.LogError("Projectile prefab or gunEnd not assigned in Drone!");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Drone hit!");
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Drone destroyed!");
        // Destroy(gameObject);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        m_IsAlive = false;

        m_DroneLight.enabled = false;
    }
}
