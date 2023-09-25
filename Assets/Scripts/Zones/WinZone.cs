using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class WinZone : MonoBehaviour
{
    [SerializeField] LayerMask m_PlayerLayer;
    void Awake()
    { 
    }

    void OnTriggerEnter(Collider other)
    {
        if((m_PlayerLayer.value & (1<<other.gameObject.layer))!=0){
            EventManager.Instance.Raise(new FinishLevelEvent());
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

