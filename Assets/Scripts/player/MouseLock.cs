using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLock : MonoBehaviour
{
    private bool m_MouseLocked = false;
    public PauseMenuScript pauseMenuScript;
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
        LockMouse();
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
            return;
        }

        // Lock or unlock mouse based on input
        if (!m_MouseLocked && Input.GetMouseButtonDown(0)) {
            LockMouse();
        }
        
        if (m_MouseLocked && Input.GetKeyDown(KeyCode.Escape)) {
            UnlockMouse();
            pauseMenuScript.pauseGame();
        }
    }
}