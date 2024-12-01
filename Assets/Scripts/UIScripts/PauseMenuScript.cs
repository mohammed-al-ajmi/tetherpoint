using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject pauseButton;

    private bool isPaused = false;

    public bool IsPaused
    {
        get { return isPaused; }
    }

    public void showDeathMenu()
    {
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
        pauseButton.SetActive(false);
        isPaused = true;
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        isPaused = true;
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        isPaused = false;
    }

    public void quitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void startGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}