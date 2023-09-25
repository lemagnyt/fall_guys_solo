using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class ChangeBodyColor : MonoBehaviour
{
    Button m_Button;
    Color m_Color;

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
        // Récupérer la couleur du bouton
        Color buttonColor = m_Button.image.color;

        // Modifier la couleur du matériau de l'objet
        EventManager.Instance.Raise(new ChangeBodyColorEvent(buttonColor));
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
    }
}
