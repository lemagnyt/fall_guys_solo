using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class GrabObject : MonoBehaviour
{
    bool m_IsGrabed;
    Rigidbody m_Rigidbody;
    GameObject m_Player;
    MotionController m_MotionPlay;
    Animator m_AnimatorPlay;
    Vector3 m_SpawnPosition;
    Quaternion m_SpawnRotation;
    [SerializeField] LayerMask m_PlayerLayer;

    private void OnTriggerEnter(Collider collision)
    {
        if(m_IsGrabed && (m_PlayerLayer.value & (1 << collision.gameObject.layer)) == 0) IsNotGrabed() ;
    }
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_SpawnPosition = transform.position;
        m_SpawnRotation = transform.rotation;
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<RespawnObjectEvent>(Respawn);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<RespawnObjectEvent>(Respawn);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    void Respawn(RespawnObjectEvent e)
    {
        if(e.respawnObject == this.gameObject){
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
            transform.rotation = m_SpawnRotation;
            transform.position = m_SpawnPosition;
            if(m_IsGrabed)IsNotGrabed();
        }
    }
    public void CanGrab(GameObject player)
    {
        m_Player = player;
        m_MotionPlay = m_Player.GetComponent<MotionController>();
        m_AnimatorPlay = m_Player.GetComponent<Animator>();
        if (!m_MotionPlay.CanGrab) m_MotionPlay.CanGrab = true;
        m_MotionPlay.GrabObjects.Add(this.gameObject);
        if (!m_IsGrabed && m_MotionPlay.IsGrabing)IsGrabed(); 
    }

    public void CanNotGrab(GameObject player) 
    { 
        if(!m_IsGrabed)
            m_MotionPlay.GrabObjects.Remove(this.gameObject);
            if (m_MotionPlay.GrabObjects.Count == 0) m_MotionPlay.CanGrab = false;
            m_Player = null;
            m_MotionPlay = null;
            m_AnimatorPlay = null;
    }

    public void IsNotGrabed()
    {
        m_IsGrabed = false;
        transform.SetParent(null);
        m_Rigidbody.isKinematic = false;
        GetComponent<BoxCollider>().isTrigger = false;
        m_MotionPlay.IsGrabing = false;
        m_MotionPlay.GrabTimerOn();
        m_AnimatorPlay.SetBool("isGrabing", false);
    }

    public void IsGrabed()
    {
        m_IsGrabed = true;
        transform.SetParent(m_Player.transform);
        transform.position = m_Player.transform.position
            + m_Player.transform.forward
            + m_Player.transform.up;
        transform.rotation = m_Player.transform.rotation;
        m_Rigidbody.isKinematic = true;
        GetComponent<BoxCollider>().isTrigger = true;
        //m_Rigidbody.constraints = m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        m_AnimatorPlay.SetBool("isGrabing", true);
        m_MotionPlay.IsGrabing = true;
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Grab);
    }

    public void Throw(float throwForce)
    {
        IsNotGrabed();
        m_Rigidbody.AddForce((m_Player.transform.forward + m_Player.transform.up) * throwForce, ForceMode.Impulse);
    }
}
