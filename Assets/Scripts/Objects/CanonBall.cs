using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform ballPosition;
    public float maxForce = 20f;
    public float fireIntervalMin = 2f;
    public float fireIntervalMax = 2f;

    private bool canFire = true;

    void Start()
    {
        StartCoroutine(AutoFire());
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, ballPosition.position, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        float force = maxForce;
        rb.AddForce(-ballPosition.forward * force, ForceMode.Impulse);
    }

    IEnumerator AutoFire()
    {
        while (canFire)
        {
            FireBullet();
            yield return new WaitForSeconds(Random.Range(fireIntervalMin, fireIntervalMax));
        }
    }

}