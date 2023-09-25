using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapisRoulant : MonoBehaviour
{
    [SerializeField] Vector2 m_Speed;
    MeshRenderer m_MeshRenderer;
    [SerializeField] LayerMask m_PlayerLayer;
    GameObject m_Player;
    Rigidbody m_PlayerRb;
    // Start is called before the first frame update
    void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((m_PlayerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            m_Player = collision.gameObject;
            m_PlayerRb = m_Player.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((m_PlayerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            m_Player = null;
            m_PlayerRb = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //m_MeshRenderer.material.mainTextureOffset += m_Speed * Time.fixedDeltaTime;
        if (m_Player != null)
        {
            Vector3 targetVelocity = transform.forward * m_Speed.x + transform.right * m_Speed.y;
            m_PlayerRb.AddForce(targetVelocity, ForceMode.VelocityChange);
        }
    }
}
