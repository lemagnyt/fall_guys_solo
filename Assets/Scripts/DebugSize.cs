using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(this.gameObject+" y : "+ transform.position.y);
        Debug.Log(this.gameObject+" y scale : "+ transform.localScale.y);
    }
}
