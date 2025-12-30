using System.Collections;
using UnityEngine;

public class StalkerController : MonoBehaviour
{
    [Header("Movement Specifications")]
    [SerializeField] private float m_moveSpeed = 1.0f;
    [SerializeField] private Vector3 m_direction = Vector3.forward;

    [Header("Colliding Box Specifications")]
    [SerializeField] private GameObject m_collisionZone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the stalker gameobject
        transform.Translate(m_direction * m_moveSpeed * Time.deltaTime);
    }

}
