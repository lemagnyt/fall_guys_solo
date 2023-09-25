using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    [SerializeField] float m_RotationSpeed;

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        // Calculer la rotation en fonction de l'axe horizontal
        float rotationAmount = hInput * m_RotationSpeed * Time.deltaTime;

        // Appliquer la rotation au personnage
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
