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

    private Transform player; // Reference to the player
    private Vector3 idleTarget; // Random target for idle movement
    private bool isCharging = false; // Whether the Outcast is currently charging

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(IdleMovement());
    }

    void Update()
    {
        if (isCharging) return;

        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < ambushRange)
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
        if (!isCharging)
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
        GetComponent<Rigidbody>().velocity = chargeDirection * chargeSpeed; // Add force for a fast charge
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
