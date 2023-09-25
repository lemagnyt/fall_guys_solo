using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class ShowLevel : MonoBehaviour
{
    Text m_Text;
    void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ShowLevelEvent>(ShowingLevel);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ShowLevelEvent>(ShowingLevel);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
    void ShowingLevel(ShowLevelEvent e)
    {
        m_Text.enabled = true;

        m_Text.text = e.level;
    }
}
