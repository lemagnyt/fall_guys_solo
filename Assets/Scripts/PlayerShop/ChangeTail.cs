using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class ChangeTail : MonoBehaviour
{
    [SerializeField] Text m_ButtonText;
    [SerializeField] GameObject m_Tail;
    Button m_Button;

    void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnButtonPressed);
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
    public void OnButtonPressed()
    {
        if (!m_Tail.GetComponent<SkinnedMeshRenderer>().enabled) m_ButtonText.text = "INVISIBLE";
        else m_ButtonText.text = "VISIBLE";
        EventManager.Instance.Raise(new ChangeTailEvent());
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
    }

}
