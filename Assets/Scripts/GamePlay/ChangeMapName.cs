using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

public class ChangeMapName : MonoBehaviour
{
    Text m_Text;
    public void Awake()
    {
        m_Text = GetComponent<Text>();
    }
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ChangeMapNameEvent>(ChangeName);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ChangeMapNameEvent>(ChangeName);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
    void ChangeName(ChangeMapNameEvent e)
    {
        m_Text.text = e.name;
    }
}
