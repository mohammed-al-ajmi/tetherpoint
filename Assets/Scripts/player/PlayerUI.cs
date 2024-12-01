using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI healthText; // Reference to UI text
    private PlayerHealth playerHealth;
    [SerializeField] private UIDocument uiDocument;
    private VisualElement healthBarForeground;

    void Start()
    {
        // Find the player and get its health script
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found on the Player!");
        }

        // Get the root VisualElement
        var root = uiDocument.rootVisualElement;

        // Fetch the health bar foreground
        healthBarForeground = root.Q<VisualElement>("Foreground");
    }

    public string updateHealthBar(int currentHealth)
    {
        healthBarForeground.style.width = new Length(currentHealth, LengthUnit.Percent);
        return "HealthBar UI updated";
    } 
}
