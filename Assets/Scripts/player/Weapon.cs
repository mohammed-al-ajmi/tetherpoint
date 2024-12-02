using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject m_Arm;
    private Light m_Flash;
    public int damage = 10;
    private Camera playerCamera;
    public AudioSource audioSource;

    void Start()
    {
        m_Arm = GameObject.Find("Arm");
        m_Flash = GameObject.Find("MuzzleFlash").GetComponent<Light>();
        m_Flash.enabled = false;

        // Find the player camera
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform cameraTransform = player.transform.Find("PlayerCamera");
        playerCamera = cameraTransform.GetComponent<Camera>();

        // Ensure audioSource is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component missing from weapon!");
            }
        }
    }

    private float m_Timer = 1.0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Flash.enabled = true;
            m_Timer = 1.0f;
            m_Arm.transform.eulerAngles += new Vector3(0.0f, 25.0f, 0.0f);

            // Play shooting sound
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            // Fire the weapon
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

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Enemy hit!");

                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}