using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    [SerializeField] LayerMask m_PlayerLayer;
    GameObject m_Player;
    Vector3 m_LastPosition;
    Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((m_PlayerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            m_Player = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((m_PlayerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            m_Player = null;
        }
    }

    private void FixedUpdate()
    {
       if(m_Player != null)
       {
            Rigidbody playerRb = m_Player.GetComponent<Rigidbody>();
            playerRb.position += m_Rigidbody.position - m_LastPosition;
       }
       m_LastPosition = m_Rigidbody.position;
    }

    private void OnDisable()
    {
        if (m_Player != null)
        {
            m_Player = null;
        }
        m_Player = null;
    }
}
