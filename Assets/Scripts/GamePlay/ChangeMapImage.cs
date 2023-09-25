using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

public class ChangeMapImage : MonoBehaviour
{
    Image m_Image;
    public void Awake()
    {
        m_Image = GetComponent<Image>();
    }
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ChangeMapImageEvent>(ChangeImage);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ChangeMapImageEvent>(ChangeImage);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
    void ChangeImage(ChangeMapImageEvent e)
    {
        m_Image.sprite = e.image;
    }
}
