using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float m_Health = 100.0f;
    public Image healthBar;

    public PauseMenuScript pauseMenuScript; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("K key pressed");
            TakeDamage(30.0f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("H key pressed");
            Heal(20.0f);
        }

        if (m_Health <= 0)
        {
            if (!pauseMenuScript.IsPaused)
            {
                pauseMenuScript.showDeathMenu();
            }
        }     
        
    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;
        healthBar.fillAmount = m_Health / 100.0f;
        
        if (m_Health <= 0)
        {
            m_Health = 0;
            pauseMenuScript.showDeathMenu();
        }
    }

    public void Heal(float healAmount)
    {
        m_Health += healAmount;
        m_Health = Mathf.Clamp(m_Health, 0, 100.0f); 
        healthBar.fillAmount = m_Health / 100.0f;
    }
}