using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI healthText; // Reference to UI text
    private PlayerHealth playerHealth;

    void Start()
    {
        // Find the player and get its health script
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found on the Player!");
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            healthText.text = $"Health: {playerHealth.GetCurrentHealth()}"; // Update health display
        }
    }
}
