using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueBoat : MonoBehaviour
{
    public GameObject m_passenger = null; // Reference to the passenger
    public ParticleSystem m_RescueAura; // Particle system for rescue visual effect
    public ParticleSystem m_Fireworks; // Particle system for fireworks when dropping off

    private float m_timer = 0; // Timer for rescue and drop-off durations
    public float m_rescueTime = 3; // Time needed to rescue a swimmer
    public float m_dropoffTime = 3; // Time needed to drop off a swimmer

    private bool m_hasPassenger = false; // Flag to check if there is a passenger

    void Start()
    {
        m_passenger.SetActive(false); // Initially hide passenger
        m_hasPassenger = false; // No passenger at start
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Swimmer" && !m_hasPassenger)
        {
            m_timer = 0; // Reset timer on encounter
            m_RescueAura.Play(); // Play rescue effect
        }
        else if (collider.gameObject.tag == "DropZone" && m_hasPassenger)
        {
            m_timer = 0; // Reset timer on drop zone encounter
            m_RescueAura.Play(); // Continue rescue effect
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Swimmer" && !m_hasPassenger)
        {
            m_timer += Time.deltaTime; // Increment timer
            if (m_timer >= m_rescueTime) // Check if rescue time is met
            {
                PickupSwimmer(collider.gameObject); // Rescue swimmer
                m_RescueAura.Stop(); // Stop rescue effect
            }
        }
        else if (collider.gameObject.tag == "DropZone" && m_hasPassenger)
        {
            m_timer += Time.deltaTime; // Increment timer
            if (m_timer >= m_dropoffTime) // Check if drop-off time is met
            {
                DropoffSwimmer(); // Drop off swimmer
                m_RescueAura.Stop(); // Stop rescue effect
                m_Fireworks.Play(); // Play fireworks
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_timer = 0; // Reset timer when leaving trigger
        m_RescueAura.Stop(); // Stop effects
    }

    public void PickupSwimmer(GameObject swimmer)
    {
        swimmer.SetActive(false); // Deactivate swimmer
        m_passenger.SetActive(true); // Activate passenger
        m_hasPassenger = true; // Set passenger flag
    }

    public void DropoffSwimmer()
    {
        m_passenger.SetActive(false); // Deactivate passenger
        m_hasPassenger = false; // Clear passenger flag
    }
}
