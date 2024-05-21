using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RescueBoat : MonoBehaviour
{
    public GameObject m_passenger = null;
    public ParticleSystem m_RescueAura;
    public ParticleSystem m_Fireworks;

    private float m_timer = 0;
    public float m_rescueTime = 3;
    public float m_dropoffTime = 3;

    private bool m_hasPassenger = false;

    void Start()
    {
        m_passenger.SetActive(false);
        m_hasPassenger = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Swimmer" && m_hasPassenger == false)
        {
            m_timer = 0;
            m_RescueAura.Play();
        }
        else if (collider.gameObject.tag == "DropZone" && m_hasPassenger == true)
        {
            m_timer = 0;
            m_RescueAura.Play();
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Swimmer" && m_hasPassenger == false)
        {
            m_timer += Time.deltaTime;
            Debug.Log((m_rescueTime / m_timer));
            if (m_timer >= m_rescueTime)
            {
                PickupSwimmer(collider.gameObject);
                m_RescueAura.Stop();
            }
        }
        else if (collider.gameObject.tag == "DropZone" && m_hasPassenger == true)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= m_dropoffTime)
            {
                DropoffSwimmer();
                m_RescueAura.Stop();
                m_Fireworks.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_timer = 0;
        m_RescueAura.Stop(other);
    }

    public void PickupSwimmer(GameObject swimmer)
    {
        swimmer.SetActive(false);
        m_passenger.SetActive(true);
        m_hasPassenger = true;
    }

    public void DropoffSwimmer()
    {
        m_passenger.SetActive(false);
        m_hasPassenger = false;
    }
}
