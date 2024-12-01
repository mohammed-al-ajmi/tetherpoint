using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI healthText; // Reference to UI text
    private PlayerHealth playerHealth;
    [SerializeField] private UIDocument HealthUI;
    [SerializeField] private UIDocument deathScreen;
    private VisualElement healthBarForeground;

    void Start()
    {
        // Find the player and get its health script
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found on the Player!");
        }

        // Get the root of health VisualElement
        var root = HealthUI.rootVisualElement;

        // Fetch the health bar foreground
        healthBarForeground = root.Q<VisualElement>("Foreground");

        
        deathScreen.gameObject.SetActive(false);
    }

    public string UpdateHealthBar(int currentHealth)
    {
        healthBarForeground.style.width = new Length(currentHealth, LengthUnit.Percent);
        return "HealthBar UI updated";
    }

    public void DisplayDeathMessage() {
        deathScreen.gameObject.SetActive(true);
    }
}
