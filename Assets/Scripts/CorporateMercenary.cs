using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CorporateMercenary : MonoBehaviour
{
    public int health = 10; // Enemy health
    public float damage = 10f; // Damage dealt to player
    public float attackRange = 2f; // Range at which the enemy attacks

    private Transform player; // Reference to the player's position
    private NavMeshAgent agent; // NavMeshAgent component
    private bool isDead = false;

    void Start()
    {
        // Get the player reference
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isDead) return;

        // Move toward the player
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            // Stop moving and attack the player
            agent.SetDestination(transform.position);
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Attacking Player");
        // Add logic to reduce player's health
        // Example:
        // player.GetComponent<PlayerHealth>().TakeDamage(damage);
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
        isDead = true;
        agent.isStopped = true;
        Debug.Log("Enemy died");
        Destroy(gameObject, 2f); // Delay for death animation
    }
}
