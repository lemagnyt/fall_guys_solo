using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushScript : MonoBehaviour
{
    [SerializeField] Transform Player1;
    [SerializeField] LayerMask m_PushLayer;
    [SerializeField] LayerMask m_HandleLayer;
    [SerializeField] LayerMask m_GrabLayer;
    private MotionController m_MotionScript;
    void Awake()
    {
        m_MotionScript = Player1.GetComponent<MotionController>(); 
    }

    void OnTriggerEnter(Collider other)
    {
        if(!m_MotionScript.IsPushing && (m_PushLayer.value & (1<<other.gameObject.layer))!=0){
            m_MotionScript.CanPush = true;
            m_MotionScript.PushObject = other.gameObject; 
        }
        if(!m_MotionScript.IsHandling && (m_HandleLayer.value & (1<<other.gameObject.layer))!=0){
            m_MotionScript.CanHandle = true;
            m_MotionScript.HandleObject = other.gameObject;
        }
        if (!m_MotionScript.IsGrabing && (m_GrabLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            other.GetComponent<GrabObject>().CanGrab(transform.parent.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if((m_PushLayer.value & (1<<other.gameObject.layer))!=0){
            if (!m_MotionScript.IsPushing || m_MotionScript.PushObject == other.gameObject)
            {
                m_MotionScript.CanPush = false;
                m_MotionScript.PushObject = null;
            }
        }
        if((m_HandleLayer.value & (1<<other.gameObject.layer))!=0){
            if (!m_MotionScript.IsHandling || m_MotionScript.HandleObject == other.gameObject)
            {
                m_MotionScript.CanHandle = false;
                m_MotionScript.HandleObject = null;
            }
        }
        if (!m_MotionScript.IsGrabing && (m_GrabLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            other.GetComponent<GrabObject>().CanNotGrab(transform.parent.gameObject);
        }
    }
}

