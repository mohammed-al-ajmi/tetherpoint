using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour, IDamageable
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

        agent.speed = speed;
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        // Move toward the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath(); // Stop moving when in range
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
       PlayerStats playerStats = player.GetComponent<PlayerStats>();
       if (playerStats != null)
       {
           playerStats.TakeDamage(damage); // Apply damage to the player
          Debug.Log($"Minion dealt {damage} damage to the player!");
      }
      else
     {
         Debug.LogError("PlayerStats component missing on Player!");
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
        Debug.Log("Minion destroyed!");
        Destroy(gameObject);
    }
}
