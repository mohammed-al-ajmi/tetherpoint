using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private Button startButton;
    private Button quitButton;

    void OnEnable()
    {
        // Get the root VisualElement
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find buttons by their names or IDs
        startButton = root.Q<Button>("Start");
        quitButton = root.Q<Button>("Quit");

        // Register callback methods for button clicks
        if (startButton != null)
        {
            startButton.clicked += OnStartButtonClicked;
        }

        if (quitButton != null)
        {
            quitButton.clicked += OnQuitButtonClicked;
        }
    }

    void OnStartButtonClicked()
    {
        // Load the first scene (assuming scene index 1 is your first game scene)
        SceneManager.LoadScene(1);
    }

    void OnQuitButtonClicked()
{
    // Quit the application
    Application.Quit();

    // If running in the Unity Editor, stop playing
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
}
}