using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Weapon : MonoBehaviour
{
    private GameObject m_Arm;

    private Light m_Flash;

    public int damage = 10;
    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        m_Arm = GameObject.Find("Arm");

        m_Flash = GameObject.Find("MuzzleFlash").GetComponent<Light>();
        m_Flash.enabled = false;

        // Find the player camera
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform cameraTransform = player.transform.Find("PlayerCamera");
        playerCamera = cameraTransform.GetComponent<Camera>();
    }

    private float m_Timer = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Flash.enabled = true;
            m_Timer = 1.0f;
            m_Arm.transform.eulerAngles += new Vector3(0.0f, 25.0f, 0.0f);

            // fire the weapon
            fire();
        }
        m_Timer -= 15.0f * Time.deltaTime;
        if (m_Timer < 0.0f)
        {
            m_Flash.enabled = false;
        }
    }

    void fire()
    {
        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera reference is missing.");
            return;
        }

        // raycast from the camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // shoot the raycast
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);

            // Check if the object hit has the Enemy layer
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Enemy hit!");

                // Try to get the IDamageable component and call TakeDamage
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }


}
