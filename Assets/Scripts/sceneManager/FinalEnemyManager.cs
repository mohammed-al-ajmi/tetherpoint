using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalEnemyManager : MonoBehaviour
{
    public List<GameObject> targetEnemies; // List of enemy GameObjects to track
    public string nextSceneName; // Name of the scene to load

    void Start()
    {
        // Initialize the list with enemies tagged as "TargetEnemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("TargetEnemy");
        targetEnemies = new List<GameObject>(enemies);
    }

    void Update()
    {
        CheckEnemies();
    }

    void CheckEnemies()
    {
        // Remove any null entries (destroyed enemies) from the list
        targetEnemies.RemoveAll(enemy => enemy == null);

        // If all target enemies are destroyed, load the next scene
        if (targetEnemies.Count == 0)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        Debug.Log("All target enemies defeated! Loading next scene.");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(6);
    }
}