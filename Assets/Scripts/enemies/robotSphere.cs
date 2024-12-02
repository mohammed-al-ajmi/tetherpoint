using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotEnemyAI : MonoBehaviour, IDamageable
{
    public Transform[] patrolPoints;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float hitRange = 2f;
    public float patrolSpeed = 3f;
    public int attackInterval = 5;
    private float lastAttackTime = 0f;
    public float chaseSpeed = 5f;
    public int health = 100;

    public int damage = 10;

    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;
    private int currentPatrolIndex;
    private bool isChasing;
    private bool isDead;

    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = patrolSpeed;
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            isChasing = true;
            anim.SetBool("Walk_Anim", true);
            anim.SetBool("Roll_Anim", false);
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else if (distanceToPlayer <= attackRange)
        {
            isChasing = false;
            anim.SetBool("Walk_Anim", false);
            if (Time.time - lastAttackTime >= attackInterval)
            {
                anim.SetBool("Roll_Anim", true);
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else if (isChasing)
        {
            isChasing = false;
            anim.SetBool("Walk_Anim", false);
            anim.SetBool("Roll_Anim", false);
            agent.speed = patrolSpeed;
        }

        if (!isChasing && patrolPoints.Length > 0)
        {
            Patrol();
        }
    }


    // Patrol between points
    void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        anim.SetBool("Walk_Anim", true);
    }

    // attack the player
    void AttackPlayer()
    {
        Debug.Log("Robot attacking player!");

        // Check colliders
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Debug.Log("Player hit by robot attack!");

                // deal damage to the player
                PlayerStats playerHealth = hitCollider.GetComponent<PlayerStats>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
                else
                {
                    Debug.LogWarning("Player does not have a PlayerHealth script!");
                }
            }
        }
    }


    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("Walk_Anim", false);
        anim.SetBool("Roll_Anim", false);
        anim.SetBool("Open_Anim", false);
        Debug.Log("Robot enemy defeated!");
        Destroy(gameObject, 2f);
    }
}
