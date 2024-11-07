using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseLock : MonoBehaviour
{
    private bool m_MouseLocked = false;

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

    // Start is called before the first frame update
    void Start()
    {
        
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


    // Update is called once per frame
    void Update()
    {
        if (!m_MouseLocked && Input.GetMouseButtonDown(0)) {
            LockMouse();
        }
        if (m_MouseLocked && Input.GetKeyDown(KeyCode.Escape)) {
            UnlockMouse();
        }
    }
}
