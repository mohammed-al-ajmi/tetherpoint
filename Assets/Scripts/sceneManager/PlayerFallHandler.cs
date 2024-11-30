
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallHandler : MonoBehaviour
{
    public float fallThreshold = -10f;

    void Update()
    {
        // check if the player has fallen below the threshold
        if (transform.position.y < fallThreshold)
        {
            ReloadScene();
        }
    }

    // reload the current scene
    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
