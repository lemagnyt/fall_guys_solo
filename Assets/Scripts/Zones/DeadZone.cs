using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class DeadZone : MonoBehaviour
{
    [SerializeField] LayerMask m_PlayerLayer;
    [SerializeField] LayerMask m_GrabLayer;
    [SerializeField] LayerMask m_BallLayer;
    void Awake()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if ((m_PlayerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Dead);
            EventManager.Instance.Raise(new DeadLevelEvent());
        }
        if ((m_GrabLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            EventManager.Instance.Raise(new RespawnObjectEvent(other.gameObject));
        }
        if ((m_BallLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(other.gameObject);
        }
    }
    public void SubscribeEvents()
    {
    }

    public void UnsubscribeEvents()
    {
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
}

