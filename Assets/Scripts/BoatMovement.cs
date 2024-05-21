using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public float m_Speed = 12f; //boat movement speed 
    public float m_RotationSpeed = 180f; //turn speed

    private Rigidbody m_Rigidbody;

    private float m_ForwardInputValue; //current movement input value
    private float m_TurnInputValue; //current turn input value

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //when the Boat is on make sure it is Not kinematic
        m_Rigidbody.isKinematic = false;

        //resets input value
        m_ForwardInputValue = 0;
        m_TurnInputValue = 0; 
    }

    private void OnDisable()
    {
        //when the Boat is turned off make kinematic
        m_Rigidbody.isKinematic = true;
    }

    private void Update()
    {
        m_ForwardInputValue = Input.GetAxisRaw("Vertical");
        m_TurnInputValue = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        //create a vector in the direction the boat is facing with a magnitude
        //based on the input speed and time between frames
        Vector3 wantedVelocity = transform.forward * m_ForwardInputValue * m_Speed;

        //apply the wantedVelocity minus the current rigidbody velocity to apply a change in the volcity on the boat
        //this ignores the mass of the boat
        m_Rigidbody.AddForce(wantedVelocity - m_Rigidbody.velocity, ForceMode.VelocityChange);
    }

    private void Turn()
    {
        //detering the number of degrees to be turned based on the input
        //speed and time between frames
        float turnValue = m_TurnInputValue * m_RotationSpeed * Time.deltaTime;

        //make this into a rotation around the y axis
        Quaternion turnRotation = Quaternion.Euler(0f, turnValue, 0f);

        //apply this rotation to the rigidbodys rotation
        m_Rigidbody.MoveRotation(transform.rotation * turnRotation);
    }
}
