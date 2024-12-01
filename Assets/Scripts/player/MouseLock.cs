using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLock : MonoBehaviour
{
    private bool m_MouseLocked = false;
    public PauseMenuScript pauseMenuScript; // Reference to your pause menu script

    public bool MouseLocked {
        get { return m_MouseLocked; }
        set { 
            if (value == true) {
                LockMouse();
            }
            else {
                UnlockMouse();   
            }
        } 
    }

    void Start()
    {
        LockMouse(); // Optionally lock the mouse at start
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_MouseLocked = true;
        Debug.Log("Lock Mouse");
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_MouseLocked = false;
    }

    void Update()
    {
        // Check if the game is paused
        if (pauseMenuScript.IsPaused)
        {
            UnlockMouse();
            return; // Exit early if paused
        }

        // Lock or unlock mouse based on input
        if (!m_MouseLocked && Input.GetMouseButtonDown(0)) {
            LockMouse();
        }
        
        if (m_MouseLocked && Input.GetKeyDown(KeyCode.Escape)) {
            UnlockMouse();
            pauseMenuScript.pauseGame(); // Trigger pause menu when unlocking
        }
    }
}