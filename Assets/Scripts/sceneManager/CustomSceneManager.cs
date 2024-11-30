using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;


    // check if the player has entered the trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player has entered the trigger");
        if (other.CompareTag("Player"))
        {
            LoadScene(sceneToLoad);
        }
    }

    // load the scene
    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("no such scene name");
        }
    }
}
