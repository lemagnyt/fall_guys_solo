using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDRotator : MonoBehaviour
{
    [SerializeField] public Vector3 rotationSpeed;
    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Obtient l'angle de rotation actuel
        Quaternion currentRotation = m_Rigidbody.rotation;
        Vector3 newRotationSpeed = rotationSpeed * Time.fixedDeltaTime;
        // Calcule la rotation souhaitée pour cet instant
        Quaternion desiredRotation = Quaternion.AngleAxis(newRotationSpeed.x, transform.forward)*Quaternion.AngleAxis(newRotationSpeed.y, transform.up) *
        Quaternion.AngleAxis(newRotationSpeed.z, transform.right)*currentRotation;
        //Quaternion desiredRotation = Quaternion.Euler(rotationSpeed.x * Time.fixedDeltaTime, rotationSpeed.y * Time.fixedDeltaTime, rotationSpeed.z * Time.fixedDeltaTime) * currentRotation;

        // Applique la rotation au rigidbody
        m_Rigidbody.MoveRotation(desiredRotation);
    }
}
