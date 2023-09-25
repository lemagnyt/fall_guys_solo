using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class touchFloor : MonoBehaviour
{
    GameObject m_Player;
    [SerializeField] LayerMask m_groundLayer;
    List<GameObject> m_Grounds;
    private MainManager m_MainScript;
    void Awake()
    {
        m_Player = transform.parent.gameObject;
        m_MainScript = m_Player.GetComponent<MainManager>();
        m_Grounds = new List<GameObject>();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<DestroyGroundEvent>(DestroyGround);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<DestroyGroundEvent>(DestroyGround);   
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void DestroyGround(DestroyGroundEvent e)
    {
        if (m_Grounds.Contains(e.ground))
        {
            m_Grounds.Remove(e.ground);
            if (m_Grounds.Count == 0) m_MainScript.IsGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if((m_groundLayer.value & (1<<other.gameObject.layer))!=0){

            m_Grounds.Add(other.gameObject);
            m_MainScript.IsGrounded = true; ;
            //Debug.Log("ON THE GROUND");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if((m_groundLayer.value & (1<<other.gameObject.layer))!=0){
            m_Grounds.Remove(other.gameObject);
            if (m_Grounds.Count == 0) m_MainScript.IsGrounded = false;
        }
    }
}
