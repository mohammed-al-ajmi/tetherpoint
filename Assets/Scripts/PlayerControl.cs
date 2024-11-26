using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float MouseSensitivity = 1000.0f;

    private Camera m_Camera;
    private CharacterController m_Controller;

    private CinemachineVirtualCamera m_FPSCamera;
    private GameObject m_CameraTarget;
    private Rigidbody m_Rigidbody;
    private GameObject m_Neck;

    private MouseLock m_MouseLock;
    private CapsuleCollider m_Collider;

    private GameObject m_Arm;

    private float m_SpeedMultiplier = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        m_FPSCamera = GameObject.Find("FPSCamera").GetComponent<CinemachineVirtualCamera>();
        m_CameraTarget = GameObject.Find("CameraTarget");
        m_Neck = GameObject.Find("Neck");
        m_Arm = GameObject.Find("Arm");

        m_MouseLock = GetComponent<MouseLock>();
        m_Collider = GetComponent<CapsuleCollider>();

        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.isKinematic = false;
    }

    void OnMouseMove(float x_axis, float y_axis)
    {
        var rotation = m_CameraTarget.transform.localEulerAngles;
        // var rotation = m_Neck.transform.localEulerAngles;

        rotation.x -= y_axis;
        rotation.y += x_axis;
        
        m_CameraTarget.transform.localEulerAngles = rotation;
        // m_CameraTarget.transform.localEulerAngles = rotation;

        UpdateArm();
    }

    bool CheckPlayerGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, m_Collider.bounds.extents.y + 0.1f);
    }

    private Vector3 lastVelocity = Vector3.zero;
    private Vector3 movement = Vector3.zero;

    void UpdatePlayerMovement()
    {
        Vector3 movementVector = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")) * m_SpeedMultiplier;
        Vector3 localMove =  1.0f * (Quaternion.Euler(0, m_FPSCamera.transform.eulerAngles.y, 0) * movementVector);
        
        // transform.position += localMove;
        // m_Rigidbody.AddForce(localMove, ForceMode.Impulse);

        Vector3 additiveVelocity = m_Rigidbody.velocity;

        // if (m_Rigidbody.velocity.magnitude > localMove.magnitude) {
        //     additiveVelocity -= lastVelocity;
        // }
        movement = localMove;
        // m_Rigidbody.AddForce(localMove, ForceMode.Force);
        m_Rigidbody.MovePosition(m_Rigidbody.position + localMove * 0.01f);
        // lastVelocity = localMove;

        // m_CameraTarget.transform.localPosition += new Vector3(0, Mathf.Sin(((float)Time.frameCount) * 5.0f * m_SpeedMultiplier * Time.deltaTime) * 0.0008f, 0);


        // TODO: replace this with the new Unity input manager way.
        float jumpAxis = Input.GetAxis("Jump");
        
        if (jumpAxis > 0.0f && CheckPlayerGrounded()) {
            Debug.Log(jumpAxis);
            m_Rigidbody.AddForce(Vector3.up * 10.0f , ForceMode.Impulse);
        }

        float sprintAxis = Input.GetAxis("Sprint");

        if (sprintAxis > 0.0f) {
            m_SpeedMultiplier = 2.0f;
            m_FPSCamera.m_Lens.FieldOfView = Mathf.Lerp(m_FPSCamera.m_Lens.FieldOfView, 95.0f, 0.008f);
        }
        else {
            m_SpeedMultiplier = 1.0f;
            m_FPSCamera.m_Lens.FieldOfView = Mathf.Lerp(m_FPSCamera.m_Lens.FieldOfView, 85.0f, 0.005f);
        }
    }

    // void FixedUpdate()
    // {
    //     m_Rigidbody.velocity = movement;
    // }

    void UpdateArm()
    {
        m_Arm.transform.rotation = Quaternion.Lerp(m_Arm.transform.rotation, m_CameraTarget.transform.rotation, 0.1f);
    }


    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;

        // only update our movement when our mouse is locked!
        // if not, when regaining focus we may accidentally process input.
        if (m_MouseLock.MouseLocked == false) {
            return;
        }

        OnMouseMove(Input.GetAxis("Mouse X") * MouseSensitivity * delta, Input.GetAxis("Mouse Y") * MouseSensitivity * delta);
        UpdatePlayerMovement();
    }
}
