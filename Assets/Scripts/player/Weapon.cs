using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Weapon : MonoBehaviour
{
    private GameObject m_Arm;

    private Light m_Flash;

    // Start is called before the first frame update
    void Start()
    {
        m_Arm = GameObject.Find("Arm");

        m_Flash = GameObject.Find("MuzzleFlash").GetComponent<Light>();   
        m_Flash.enabled = false;     
    }

    private float m_Timer = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            m_Flash.enabled = true;
            m_Timer = 1.0f;
            m_Arm.transform.eulerAngles += new Vector3(0.0f, 25.0f, 0.0f);
        }
        m_Timer -= 15.0f * Time.deltaTime;
        if (m_Timer < 0.0f) {
            m_Flash.enabled = false;
        }
    }
}
