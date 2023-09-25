using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class CheckPointZone : MonoBehaviour
{
    [SerializeField] LayerMask m_PlayerLayer;
    [SerializeField] int m_Zone;
    List<Vector3> m_Spawns;
    void Awake()
    {
        m_Spawns = new List<Vector3>();
        int nSpawns = transform.childCount;

        for (int i = 0; i < nSpawns; i++)
        {
            Transform spawnTransform = transform.GetChild(i);
            Vector3 spawnPosition = spawnTransform.position;
            m_Spawns.Add(spawnPosition);
        }
    }

    void FixedUpdate()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if ((m_PlayerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            EventManager.Instance.Raise(new NewCheckPointEvent(m_Spawns,m_Zone));
        }
    }
}
