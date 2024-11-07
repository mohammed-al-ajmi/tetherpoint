using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    private CinemachineVirtualCamera m_FPSCamera;
    private GameObject m_RaycastMarker;

    private Rigidbody m_Rigidbody;

    public bool IsPulling = false;
    private Vector3? m_Tetherpoint = null;



    // Start is called before the first frame update
    void Start()
    {
        m_FPSCamera = GameObject.Find("FPSCamera").GetComponent<CinemachineVirtualCamera>();
        m_RaycastMarker = GameObject.Find("RaycastMarker");
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FindGrapplePoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(m_FPSCamera.transform.position, m_FPSCamera.transform.TransformDirection(Vector3.forward), out hit, 100.0f)) {
            Debug.DrawRay(m_FPSCamera.transform.position, m_FPSCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); 
            m_RaycastMarker.transform.position = hit.point;

            m_Tetherpoint = hit.point;
        }
        else {
            Debug.DrawRay(m_FPSCamera.transform.position, m_FPSCamera.transform.TransformDirection(Vector3.forward) * 100, Color.white); 
            Debug.Log("Did not Hit"); 

            m_Tetherpoint = null;
        }
    }

    void UpdateGrapple()
    {
        if (m_Tetherpoint == null) {
            return;
        }
        Vector3 grappleDirection = m_Tetherpoint.Value - transform.position;
        // transform.position += Vector3.Normalize(grappleDirection) * 15.0f * Time.deltaTime;
        m_Rigidbody.AddForce(grappleDirection);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            FindGrapplePoint();
            IsPulling = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            IsPulling = false;
        }

        if (IsPulling) {
            UpdateGrapple();
        }
    }
}
