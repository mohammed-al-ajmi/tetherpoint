using UnityEngine;

public class PlayerFallHandler : MonoBehaviour
{
    public float fallThreshold = -10f;
    public PauseMenuScript pauseMenuScript; // Reference to PauseMenuScript

    void Update()
    {
        // Check if the player has fallen below the threshold
        if (transform.position.y < fallThreshold)
        {
            ShowDeathMenu();
        }
    }

    private void ShowDeathMenu()
    {
        if (pauseMenuScript != null && !pauseMenuScript.IsPaused)
        {
            pauseMenuScript.showDeathMenu();
        }
        else
        {
            Debug.LogError("PauseMenuScript reference is missing or already paused!");
        }
    }
}