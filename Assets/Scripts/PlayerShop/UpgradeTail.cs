using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class UpgradeTail : MonoBehaviour
{
    Material m_Material;
    private void Awake()
    {
        m_Material = GetComponent<SkinnedMeshRenderer>().materials[0];
    }
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GiveTailEvent>(GetTail);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GiveTailEvent>(GetTail);
    }

    void OnEnable()
    {
        SubscribeEvents();
        EventManager.Instance.Raise(new GetTailEvent());
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
    void GetTail(GiveTailEvent e)
    {
        this.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = e.tailVisible;
        m_Material.color = e.color;
    }
}
