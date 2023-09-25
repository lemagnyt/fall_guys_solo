using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorAngle : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
    private Rigidbody rb;
    private float currentAngle = 0f;
    private bool isRotatingClockwise = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Calcule la rotation souhaitée pour cet instant
        float desiredAngle = currentAngle + (isRotatingClockwise ? 1f : -1f) * rotationSpeed * Time.deltaTime;

        // Vérifie si l'angle atteint la limite minimale ou maximale
        if (desiredAngle <= minAngle || desiredAngle >= maxAngle)
        {
            // Inverse la direction de rotation
            isRotatingClockwise = !isRotatingClockwise;
        }

        // Assure que l'angle reste dans la plage spécifiée
        desiredAngle = Mathf.Clamp(desiredAngle, minAngle, maxAngle);

        // Calcule la rotation en fonction de l'angle désiré
        Quaternion desiredRotation = Quaternion.Euler(rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.y, desiredAngle);

        // Applique la rotation au rigidbody
        rb.MoveRotation(desiredRotation);

        // Met à jour l'angle actuel
        currentAngle = desiredAngle;
    }
    
}
