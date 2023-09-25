using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class DrawingBlock : MonoBehaviour
{
    [SerializeField] LayerMask m_PlayerLayer;
    bool m_Active;

    private void OnTriggerEnter(Collider other)
    {
        if ((m_PlayerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Color color = this.GetComponent<MeshRenderer>().material.color;
            if (color == Color.yellow)
            {
                this.GetComponent<MeshRenderer>().material.color = Color.grey;
                SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_DrawingBlockOff);
            }
            else
            {
                this.GetComponent<MeshRenderer>().material.color = Color.yellow;
                SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_DrawingBlockOff);
            }
            EventManager.Instance.Raise(new ChangeDrawingEvent());
        }
    }
    private void OnTriggerExit(Collider other)
    {
    }
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<InitDrawingEvent>(InitBlock);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<InitDrawingEvent>(InitBlock);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    void InitBlock(InitDrawingEvent e)
    {
        this.GetComponent<MeshRenderer>().material.color = Color.grey;
    }
}
