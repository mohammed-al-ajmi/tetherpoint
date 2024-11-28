using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class GrappleGun : MonoBehaviour
{
    private CinemachineVirtualCamera m_FPSCamera;
    private GameObject m_RaycastMarker;
    private Camera m_PlayerCamera;

    private Rigidbody m_Rigidbody;

    public bool IsPulling = false;
    private Vector3? m_Tetherpoint = null;

    private LineRenderer m_LineRenderer = null;
    private GameObject m_Hand = null;
    private int m_FrameStart = 0;



    // Start is called before the first frame update
    void Start()
    {
        m_FPSCamera = GameObject.Find("FPSCamera").GetComponent<CinemachineVirtualCamera>();
        m_RaycastMarker = GameObject.Find("RaycastMarker");
        m_Hand = GameObject.Find("HandL");
        m_LineRenderer = m_Hand.GetComponent<LineRenderer>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    void SetAllSegmentPositions()
    {
        for (int i = 1; i < m_LineRenderer.positionCount - 1; i++) {
            Vector3 expectedPos = Vector3.Lerp(m_Hand.transform.position, m_Tetherpoint.GetValueOrDefault(), i / ((float)m_LineRenderer.positionCount - 1.0f));
            m_LineRenderer.SetPosition(i, expectedPos);
        }
    }

    void FindGrapplePoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(m_FPSCamera.transform.position, m_FPSCamera.transform.TransformDirection(Vector3.forward), out hit, 100.0f)) {
            Debug.DrawRay(m_FPSCamera.transform.position, m_FPSCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); 
            // m_RaycastMarker.transform.position = hit.point;

            m_Tetherpoint = hit.point;

            m_LineRenderer.enabled = true;
            m_FrameStart = Time.frameCount;
            m_Rigidbody.AddForce(Vector3.up * 20.0f, ForceMode.Impulse);

            SetAllSegmentPositions();
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
        m_Rigidbody.AddForce(grappleDirection * 5.0f, ForceMode.Impulse);
        m_Rigidbody.maxLinearVelocity = 16.0f;

        int frame = Time.frameCount;
        float animProgress = Math.Min(m_ShotTimer, 1.0f);

        for (int i = 1; i < m_LineRenderer.positionCount - 1; i++) {
            Vector3 expectedPos = Vector3.Lerp(m_Hand.transform.position, m_Tetherpoint.GetValueOrDefault(), i / ((float)m_LineRenderer.positionCount - 1.0f));
            float amount = animProgress;


            Vector3 position = expectedPos + new Vector3(0, Mathf.Sin((frame + i) * 0.8f * (1.0f - animProgress)) * 0.5f * (1.0f - animProgress), 0);


            m_LineRenderer.SetPosition(i, Vector3.Lerp(m_Hand.transform.position, position, animProgress));

        }
        m_ShotTimer += 7.0f * Time.deltaTime;
        
        m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, Vector3.Lerp(m_Hand.transform.position, m_Tetherpoint.GetValueOrDefault(), animProgress));
        m_LineRenderer.SetPosition(0, m_Hand.transform.position);

        m_FPSCamera.m_Lens.FieldOfView = Mathf.Lerp(m_FPSCamera.m_Lens.FieldOfView, 95.0f, 0.008f);
    }


    private float m_ShotTimer = 0.0f;
    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0)) {
        if (Input.GetButtonDown("GrapplePull")) {
            Debug.Log("Grapple");
            FindGrapplePoint();
            IsPulling = true;
        }
        // if (Input.GetMouseButtonUp(0)) {
        if (Input.GetButtonUp("GrapplePull")) {
            IsPulling = false;
        }

        if (IsPulling) {
            UpdateGrapple();

            

        }
        else {
            m_ShotTimer = 0.0f;
            m_LineRenderer.enabled = false;
            m_FPSCamera.m_Lens.FieldOfView = Mathf.Lerp(m_FPSCamera.m_Lens.FieldOfView, 85.0f, 0.005f);
        }
    }
}
