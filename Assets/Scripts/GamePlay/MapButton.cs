using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    Button m_Button;
    [SerializeField] int m_MapInd;
    bool m_Active;

    void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnButtonPressed);
        m_Button.image.color = Color.red;
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<InitSelectMapEvent>(InitButton);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<InitSelectMapEvent>(InitButton);
    }

    void InitButton(InitSelectMapEvent e)
    {
        if (m_Active)
        {
            OffButton();
        }
    }

    public void OffButton()
    {
        if (m_Active)
        {
            EventManager.Instance.Raise(new RemoveSelectMapEvent(m_MapInd));
            m_Active = false;
            m_Button.image.color = Color.red;
        }
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void OnButtonPressed()
    {
        if (!m_Active) {
            EventManager.Instance.Raise(new AddSelectMapEvent(m_MapInd));
            m_Active = true;
            m_Button.image.color = Color.green;
        }
        else
        {
            OffButton();
        }
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
    }
}
