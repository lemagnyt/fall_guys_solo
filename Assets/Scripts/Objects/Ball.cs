using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] LayerMask m_ColorizableLayerMask;

    private void OnCollisionEnter(Collision collision)
    {
        // IDENTIFICATION PAR NAME
        //if(collision.gameObject.name.Equals("Cube"))
        //if (collision.gameObject.name.ToUpper().Contains("CUBE"))

        // IDENTIFICATION PAR TAG
        //if(collision.gameObject.CompareTag("Colorizable"))

        // IDENTIFICATION PAR LAYER
        //if((m_ColorizableLayerMask.value & (1<<collision.gameObject.layer))!=0)

        // IDENTIFICATION PAR COMPONENT TAG
        // if (null != collision.gameObject.GetComponent<ColorizableTag>())

        //IDENTIFICATION FONCTIONNELLE PAR INTERFACE
        IDestroyable destroyable = collision.gameObject.GetComponent<IDestroyable>();
        if (null != destroyable)
            destroyable.Kill();
    }
}
