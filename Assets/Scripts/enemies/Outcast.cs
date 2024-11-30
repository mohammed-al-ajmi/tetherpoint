using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outcast : MonoBehaviour
{
    public int health = 20; // Moderate health level
    public float speed = 5f; // Normal wandering speed
    public float chargeSpeed = 15f; // High speed during a charge
    public float ambushRange = 10f; // Distance within which the Outcast starts charging
    public float idleMoveRadius = 5f; // Radius for idle wandering
    public float idleMoveInterval = 3f; // Time between idle movement
    public float retreatDistance = 5f; // Distance to retreat after attacking
    public float chargeCooldown = 2f; // Time before the Outcast can charge again

    private Transform player; // Reference to the player
    private Vector3 idleTarget; // Random target for idle movement
    private bool isCharging = false; // Whether the Outcast is currently charging
    private bool isRetreating = false; // Whether the Outcast is retreating
    private Rigidbody rb; // Rigidbody component
    private float lastChargeTime = 0f; // Tracks the last charge time

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(IdleMovement());
    }

    void Update()
    {
        if (isCharging || isRetreating) return;

        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < ambushRange && Time.time - lastChargeTime > chargeCooldown)
        {
            StopCoroutine(IdleMovement());
            ChargePlayer();
        }
    }

    IEnumerator IdleMovement()
    {
        while (true)
        {
            // Pick a random point within the idle move radius
            idleTarget = transform.position + Random.insideUnitSphere * idleMoveRadius;
            idleTarget.y = transform.position.y; // Keep it on the same level
            yield return new WaitForSeconds(idleMoveInterval);
        }
    }

    void FixedUpdate()
    {
        if (!isCharging && !isRetreating)
        {
            // Smooth movement toward the idle target
            transform.position = Vector3.MoveTowards(transform.position, idleTarget, speed * Time.fixedDeltaTime);
        }
    }

    void ChargePlayer()
    {
        Debug.Log("Outcast charging player!");
        isCharging = true;
        Vector3 chargeDirection = (player.position - transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed; // Add velocity for a fast charge
        Invoke(nameof(StartRetreat), 1f); // Start retreating after a short delay
    }

    void StartRetreat()
    {
        if (player == null) return;

        Debug.Log("Outcast retreating!");
        isCharging = false;
        isRetreating = true;

        Vector3 retreatDirection = (transform.position - player.position).normalized;
        idleTarget = transform.position + retreatDirection * retreatDistance;
        rb.velocity = Vector3.zero; // Stop the charging velocity
        Invoke(nameof(ResumeIdle), 1f); // Resume idle movement after retreat
    }

    void ResumeIdle()
    {
        Debug.Log("Outcast resuming idle behavior.");
        isRetreating = false;
        StartCoroutine(IdleMovement());
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
        Debug.Log("Outcast defeated!");
        Destroy(gameObject);
    }
}
