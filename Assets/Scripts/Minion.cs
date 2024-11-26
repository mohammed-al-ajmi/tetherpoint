using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    public int health = 5;
    public float speed = 4f;
    public float attackRange = 1.5f;
    public int damage = 5;

    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent missing from Minion prefab!");
            return;
        }

        // Set NavMeshAgent properties
        agent.speed = speed;
        agent.acceleration = 8f; // Quick acceleration for responsive movement
        agent.angularSpeed = 120f; // Turning speed
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        // Move toward the player if within the NavMesh
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            // Stop moving and attack
            agent.ResetPath(); // Prevent jittering movement
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Minion attacking player!");
        // Placeholder for health reduction logic
        // Example: player.GetComponent<PlayerHealth>().TakeDamage(damage);
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
        Debug.Log("Minion destroyed!");
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with other minions or Syndicate Leader
        if (collision.gameObject.CompareTag("Minion") || collision.gameObject.CompareTag("SyndicateLeader"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
