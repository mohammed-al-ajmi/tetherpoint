using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject m_Arm;

    // Start is called before the first frame update
    void Start()
    {
        m_Arm = GameObject.Find("Arm");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
