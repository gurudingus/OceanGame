using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public float m_Speed = 12f; // Movement speed
    public float m_RotationSpeed = 180f; // Turning speed

    private Rigidbody m_Rigidbody;

    private float m_ForwardInputValue; // Movement input
    private float m_TurnInputValue; // Turn input

    public GameManager m_GameManager;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); // Get Rigidbody component
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false; // Set kinematic to false on enable
        m_ForwardInputValue = 0; // Reset forward input
        m_TurnInputValue = 0; // Reset turn input
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true; // Set kinematic to true on disable
    }

    private void Update()
    {
        if (m_GameManager.State == GameManager.GameState.Playing)
        {
            m_ForwardInputValue = Input.GetAxisRaw("Vertical"); // Get vertical input
            m_TurnInputValue = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        }
    }

    private void FixedUpdate()
    {
        Move(); // Process movement
        Turn(); // Process turning
    }

    private void Move()
    {
        Vector3 wantedVelocity = transform.forward * m_ForwardInputValue * m_Speed; // Calculate desired velocity
        m_Rigidbody.AddForce(wantedVelocity - m_Rigidbody.velocity, ForceMode.VelocityChange); // Apply force for movement
    }

    private void Turn()
    {
        float turnValue = m_TurnInputValue * m_RotationSpeed * Time.deltaTime; // Calculate turn value
        Quaternion turnRotation = Quaternion.Euler(0f, turnValue, 0f); // Create rotation
        m_Rigidbody.MoveRotation(transform.rotation * turnRotation); // Apply rotation
    }
}
