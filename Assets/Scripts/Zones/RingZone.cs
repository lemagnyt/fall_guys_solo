using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class RingZone : MonoBehaviour
{
    [SerializeField] LayerMask m_PlayerLayer;
    [SerializeField] float m_Time;
    [SerializeField] float m_SpeedBoost;

    private void OnTriggerEnter(Collider other)
    {
        if ((m_PlayerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_BoostSpeed);
            EventManager.Instance.Raise(new SpeedPlayerEvent(m_Time,m_SpeedBoost));
        }
    }
}
