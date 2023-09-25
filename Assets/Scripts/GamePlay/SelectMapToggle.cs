using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;
public class SelectMapToggle : MonoBehaviour
{
    [SerializeField] List<Button> m_Buttons;
    Toggle m_Toggle;
    private int m_Count;
    public bool m_CanPlay;

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<AddSelectMapEvent>(AddButton);
        EventManager.Instance.AddListener<RemoveSelectMapEvent>(RemoveButton);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<AddSelectMapEvent>(AddButton);
        EventManager.Instance.RemoveListener<RemoveSelectMapEvent>(RemoveButton);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }


    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
    }

    public void Click()
    {
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
        foreach (Button button in m_Buttons)
        {
            if (m_Toggle.isOn) button.gameObject.SetActive(true);
            else
            {
                button.GetComponent<MapButton>().OffButton();
                button.gameObject.SetActive(false);
            }
        }
        EventManager.Instance.Raise(new SelectModeEvent());

    }

    void AddButton(AddSelectMapEvent e)
    {
        m_Count += 1;
        if (m_Count == 1) m_CanPlay = true;
    }

    void RemoveButton(RemoveSelectMapEvent e)
    {
        m_Count -= 1;
        if (m_Count == 0) m_CanPlay = false;
    }
    
}
