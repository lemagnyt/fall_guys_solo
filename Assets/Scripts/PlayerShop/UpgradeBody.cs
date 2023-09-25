using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class UpgradeBody : MonoBehaviour
{
    Material m_Material;
    private void Awake()
    {
        m_Material = GetComponent<SkinnedMeshRenderer>().materials[0];
    }
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GiveBodyColorEvent>(GetBodyColor);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GiveBodyColorEvent>(GetBodyColor);
    }

    void OnEnable()
    {
        SubscribeEvents();
        EventManager.Instance.Raise(new GetBodyColorEvent());
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
    void GetBodyColor(GiveBodyColorEvent e)
    {
        m_Material.color = e.color;
    }
}
