using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outcast : MonoBehaviour, IDamageable
{
    public int health = 20;
    public float speed = 5f;
    public float chargeSpeed = 15f;
    public float ambushRange = 20.0f;
    public float idleMoveRadius = 5f;
    public float idleMoveInterval = 3f;
    public float retreatDistance = 5f;
    public float chargeCooldown = 2f;

    private Transform player;
    private Vector3 idleTarget;
    private bool isCharging = false;
    private bool isRetreating = false;
    private Rigidbody rb;
    private float lastChargeTime = 0f;

    public int damage = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(IdleMovement());
    }

    void Update()
    {
        if (isCharging || isRetreating) return;

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
            idleTarget = transform.position + Random.insideUnitSphere * idleMoveRadius;
            idleTarget.y = transform.position.y;
            yield return new WaitForSeconds(idleMoveInterval);
        }
    }

    void FixedUpdate()
    {
        if (!isCharging && !isRetreating)
        {
            transform.position = Vector3.MoveTowards(transform.position, idleTarget, speed * Time.fixedDeltaTime);
        }
    }

    void ChargePlayer()
    {
        Debug.Log("Outcast charging player!");
        isCharging = true;
        Vector3 chargeDirection = (player.position - transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed;
        Invoke(nameof(StartRetreat), 1f);
    }

    void StartRetreat()
    {
        Debug.Log("Outcast retreating!");
        
        if (player == null) return;

        isCharging = false;
        isRetreating = true;

        Vector3 retreatDirection = (transform.position - player.position).normalized;
        rb.velocity = retreatDirection * speed; // Use speed for controlled retreat
        Invoke(nameof(ResumeIdle), 1f); // Adjust time as needed
    }

    void ResumeIdle()
    {
        Debug.Log("Outcast resuming idle behavior.");
        isRetreating = false;
        StartCoroutine(IdleMovement());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Outcast hit the player!");

            PlayerStats playerHealth = collision.gameObject.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("Player does not have a PlayerStats script!");
            }

            StartRetreat();
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
        Debug.Log("Outcast defeated!");
        Destroy(gameObject);
    }
}