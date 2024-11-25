using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SyndicateLeader : MonoBehaviour
{
    public int health = 100;
    public float speed = 3f;
    public float summonInterval = 10f;
    public GameObject projectilePrefab;
    public GameObject minionPrefab;
    public Transform summonPoint;

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

        // Move toward the player
        agent.SetDestination(player.position);

        // Summon minions periodically
        if (Time.time - lastSummonTime > summonInterval)
        {
            SummonMinions();
            lastSummonTime = Time.time;
        }

        // Attack with projectiles
        if (projectilePrefab != null)
        {
            AttackPlayer();
        }
    }

    void SummonMinions()
    {
        Debug.Log("Summoning minions...");
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 2f;
            randomOffset.y = 0; // Ensure minions spawn on the ground
            Vector3 spawnPosition = summonPoint.position + randomOffset;

            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                GameObject minion = Instantiate(minionPrefab, hit.position, Quaternion.identity);
                minion.tag = "Minion"; // Set tag for collision checks
            }
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Syndicate Leader attacking player!");
        GameObject projectile = Instantiate(projectilePrefab, summonPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (player.position - summonPoint.position).normalized * 10f; // Fire toward the player
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
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

    void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with minions
        if (collision.gameObject.CompareTag("Minion"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
