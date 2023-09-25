using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour
{
    private int m_NObject;
    [SerializeField] int m_NObjectGoal;
    [SerializeField] LayerMask m_GrabLayer;
    [SerializeField] GameObject m_Door;

    private void Awake()
    {
        m_NObject = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((m_GrabLayer.value & (1 << other.gameObject.layer)) != 0) 
        {
            m_NObject += 1;
            if(m_NObject == m_NObjectGoal)
            {
                SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_OpenDoor);
                Destroy(m_Door);
            }
        }
    }
}
