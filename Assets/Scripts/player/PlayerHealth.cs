using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private PlayerUI playerUI;
    float timer = 0;

    void Start()
    {
        currentHealth = maxHealth;
        playerUI = GetComponent<PlayerUI>();
    }


    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
            Debug.Log($"Player Health: {currentHealth}");
            playerUI.UpdateHealthBar(currentHealth);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerUI.DisplayDeathMessage();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
