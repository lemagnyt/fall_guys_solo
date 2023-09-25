using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGround : MonoBehaviour
{
    [SerializeField] LayerMask m_PlayerLayer;
    Rigidbody m_Rigidbody;
    GameObject m_Player;
    Rigidbody m_PlayerRb;
    MotionController m_PlayerMotion;
    WNDRotator m_Rotator;
    bool m_Active;
    // Start is called before the first frame update
    void Awake()
    {
        m_Rotator = GetComponent<WNDRotator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (m_Active)
        {
            GameObject parent = m_PlayerMotion.ParentObject;
            if (parent == null) m_PlayerMotion.ParentObject = this.gameObject;
            if (parent == this.gameObject)
            {
                Vector3 rotationAmount = m_Rotator.rotationSpeed * Time.fixedDeltaTime;
                Quaternion localAngleAxis = Quaternion.AngleAxis(rotationAmount.x, m_Rigidbody.transform.forward);
                localAngleAxis *= Quaternion.AngleAxis(rotationAmount.y, m_Rigidbody.transform.up);
                localAngleAxis *= Quaternion.AngleAxis(rotationAmount.z, m_Rigidbody.transform.right);
                m_PlayerRb.position = (localAngleAxis * (m_PlayerRb.position - m_Rigidbody.position)) + m_Rigidbody.position;
                Quaternion globalAngleAxis = Quaternion.AngleAxis(rotationAmount.x, m_Player.transform.InverseTransformDirection(m_Rigidbody.transform.forward));
                globalAngleAxis *= Quaternion.AngleAxis(rotationAmount.y, m_Player.transform.InverseTransformDirection(m_Rigidbody.transform.up));
                globalAngleAxis *= Quaternion.AngleAxis(rotationAmount.z, m_Player.transform.InverseTransformDirection(m_Rigidbody.transform.right));
                m_PlayerMotion.LocalAngle = globalAngleAxis;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((m_PlayerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            m_Active = true;
            m_Player = collision.gameObject;
            m_PlayerRb = m_Player.GetComponent<Rigidbody>();
            m_PlayerMotion = m_Player.GetComponent<MotionController>();
            m_PlayerMotion.ParentObject = this.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((m_PlayerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            m_Active = false;
            m_Player = null;
            m_PlayerRb = null;
            if (m_PlayerMotion.ParentObject == this.gameObject)
            {
                m_PlayerMotion.ParentObject = null;
            }
            m_PlayerMotion = null;
        }
    }
}
