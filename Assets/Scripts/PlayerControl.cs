using Cinemachine;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float MouseSensitivity = 1000.0f;

    private Camera m_Camera;
    private CharacterController m_Controller;

    private CinemachineVirtualCamera m_FPSCamera;
    private Rigidbody m_Rigidbody;

    private MouseLock m_MouseLock;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        m_FPSCamera = GameObject.Find("FPSCamera").GetComponent<CinemachineVirtualCamera>();

        m_MouseLock = GetComponent<MouseLock>();

        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.isKinematic = false;
        
    }

    void OnMouseMove(float x_axis, float y_axis)
    {
        var rotation = transform.localEulerAngles;

        rotation.x -= y_axis;
        rotation.y += x_axis;
        
        transform.localEulerAngles = rotation;
    }

    void UpdatePlayerMovement()
    {
        Vector3 movementVector = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Vector3 localMove =  10.0f * (Quaternion.Euler(0, m_FPSCamera.transform.eulerAngles.y, 0) * movementVector) * Time.deltaTime;
        
        transform.position += localMove;
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
