using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjectScript : MonoBehaviour
{
    void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
