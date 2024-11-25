using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outcast : MonoBehaviour
{
    public int health = 20;
    public float speed = 5f;
    public float chargeSpeed = 15f;
    public float ambushRange = 10f;
    public float idleMoveRadius = 5f;
    public float idleMoveInterval = 3f;

    private Transform player;
    private Vector3 idleTarget;
    private bool isCharging = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(IdleMovement());
    }

    void Update()
    {
        if (isCharging) return;

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
            idleTarget = transform.position + Random.insideUnitSphere * idleMoveRadius;
            idleTarget.y = transform.position.y;
            yield return new WaitForSeconds(idleMoveInterval);
        }
    }

    void FixedUpdate()
    {
        if (!isCharging)
        {
            // Idle wandering
            transform.position = Vector3.MoveTowards(transform.position, idleTarget, speed * Time.fixedDeltaTime);
        }
    }

    void ChargePlayer()
    {
        Debug.Log("Outcast charging player!");
        isCharging = true;
        Vector3 chargeDirection = (player.position - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = chargeDirection * chargeSpeed;
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
