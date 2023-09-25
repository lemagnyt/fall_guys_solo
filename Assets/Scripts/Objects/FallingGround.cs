using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class FallingGround : MonoBehaviour
{
    bool m_Fall;
    [SerializeField] LayerMask m_PlayerLayer;
    Material m_Material;
    private void Awake()
    {
        m_Material = GetComponent<MeshRenderer>().material;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if((m_PlayerLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (!m_Fall)
            {
                SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_DrawingBlockOn);
                StartCoroutine(Fall());
            }
        }
    }

    IEnumerator Fall()
    {
        m_Fall = true;
        m_Material.color = Color.yellow;
        yield return new WaitForSeconds(0.5f);
        m_Material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        EventManager.Instance.Raise(new DestroyGroundEvent(gameObject));
        this.gameObject.SetActive(false);
    }
}
