using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f; // Time to dampen camera movement

    public Transform m_target; // Target for the camera to follow

    private Vector3 m_MoveVelocity; // Current velocity of the camera
    private Vector3 m_DesiredPosition; // Desired position of the camera

    private void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform; // Assign target based on player tag
    }

    private void FixedUpdate()
    {
        Move(); // Move camera every physics update
    }

    private void Move()
    {
        m_DesiredPosition = m_target.position; // Update desired position to target's position
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime); // Smoothly move camera
    }
}
