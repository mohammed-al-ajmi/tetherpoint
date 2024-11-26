using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SyndicateLeader : MonoBehaviour
{
    public int health = 100; // Highest health among enemies
    public float speed = 3f; // Movement speed
    public float summonInterval = 10f; // Time between minion summons
    public GameObject projectilePrefab; // Projectile for ranged attack
    public GameObject minionPrefab; // Minion to summon
    public Transform summonPoint; // Point where minions are spawned
    public Transform gunEnd; // Point where projectiles are fired

    private Transform player;
    private float lastSummonTime;
    private NavMeshAgent agent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        // Chase the player
        agent.SetDestination(player.position);

        // Summon minions periodically
        if (Time.time - lastSummonTime > summonInterval)
        {
            SummonMinions();
            lastSummonTime = Time.time;
        }

        // Attack the player with projectiles
        AttackPlayer();
    }

    void SummonMinions()
    {
        Debug.Log("Summoning minions...");
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 2f;
            randomOffset.y = 0; // Keep minions grounded
            Vector3 spawnPosition = summonPoint.position + randomOffset;

            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                Instantiate(minionPrefab, hit.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid spawn position for minion!");
            }
        }
    }

    void AttackPlayer()
    {
        if (projectilePrefab != null && gunEnd != null)
        {
            // Fire a projectile every second
            if (Time.time - lastSummonTime > 1f)
            {
                Debug.Log("Syndicate Leader firing at player!");
                GameObject projectile = Instantiate(projectilePrefab, gunEnd.position, Quaternion.identity);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.velocity = (player.position - gunEnd.position).normalized * 50f; // Set projectile speed
                lastSummonTime = Time.time;
            }
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
        Debug.Log("Syndicate Leader defeated!");
        Destroy(gameObject);
    }
}
