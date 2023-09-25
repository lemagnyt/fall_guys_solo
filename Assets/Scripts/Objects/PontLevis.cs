using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontLevis : MonoBehaviour
{
    [SerializeField] LayerMask m_PlayerLayer;
    GameObject m_Player;
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
        if (m_Player != null)
        {
            Rigidbody playerRb = m_Player.GetComponent<Rigidbody>();

            playerRb.AddForce(-(90f - Vector3.Angle(Vector3.up, transform.right)) * Vector3.up*2, ForceMode.Force);
        }
    }
}
