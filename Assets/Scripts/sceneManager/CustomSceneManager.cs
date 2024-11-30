using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string enemyLayer = "functionalEnemy"; // layer

    // check if the player has entered the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // if all enemies are dead, load the scene
            if (AreAllEnemiesDead())
            {
                LoadScene(sceneToLoad);
            }
            else
            {
                Debug.Log("Not all enemies are dead");
            }
        }
    }

    // check if all enemies are dead
    private bool AreAllEnemiesDead()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer(enemyLayer);
        Collider[] enemies = Physics.OverlapSphere(transform.position, Mathf.Infinity, enemyLayerMask);

        return enemies.Length == 0;
    }

    // Load the scene
    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("No such scene name");
        }
    }
}
