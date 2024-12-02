using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Transform teleportDestination;

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Teleport the player to the new location
            other.transform.position = teleportDestination.position;
            Debug.Log("Player teleported to: " + teleportDestination.position);
        }
    }
}