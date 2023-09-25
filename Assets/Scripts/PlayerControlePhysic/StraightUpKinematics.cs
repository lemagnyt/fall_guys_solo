using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightUpKinematics : MonoBehaviour
{
    [SerializeField] float m_RotationSpeed;
    Rigidbody m_Rigidbody;
    private MainManager m_MainScript;
    bool isStraightUp;
    private Coroutine m_StraightUpCoroutine;
    private Animator m_Animator;
    private float m_TimeToStandUp;

    public bool IsStraightUp
    {
        get { return isStraightUp; }
        set { isStraightUp = value; }
    }
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_MainScript = GetComponent<MainManager>();
        isStraightUp = false;
        m_Animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_TimeToStandUp < Time.time) BugStandUp();
    }
    IEnumerator StartStandingUp()
    {
        Vector3 startPosition = transform.position;
        Quaternion endRotation = Quaternion.FromToRotation(transform.up, m_MainScript.m_GroundNormal);
        //m_Rigidbody.useGravity = false;
        //yield return StartCoroutine(MyTools.TranslationCoroutine(transform,startPosition,startPosition+m_MainScript.m_GroundNormal.normalized * 0.7f,4f));
        yield return StartCoroutine(MyTools.RotationCoroutine(m_Rigidbody, transform, transform.rotation, endRotation, m_RotationSpeed));
        //m_Rigidbody.useGravity = true;
        isStraightUp = true;
        m_StraightUpCoroutine = null;
    }

    private void OnEnable()
    {
        m_Rigidbody.AddForce(-m_Rigidbody.velocity, ForceMode.VelocityChange);
        m_StraightUpCoroutine = StartCoroutine(StartStandingUp());
        m_TimeToStandUp = Time.time + 5f;
        m_Animator.SetBool("isStandingUp", true);
    }

    private void OnDisable()
    {
        isStraightUp = false;
        if (m_StraightUpCoroutine != null)
        {
            StopCoroutine(m_StraightUpCoroutine);
            m_StraightUpCoroutine = null;
        }
        m_Animator.SetBool("isStandingUp", false);
        m_TimeToStandUp = 0;
    }

    void BugStandUp()
    {
        transform.position += m_MainScript.m_GroundNormal * transform.localScale.y;
        m_Rigidbody.MoveRotation(Quaternion.FromToRotation(transform.up, m_MainScript.m_GroundNormal) * transform.rotation);
        isStraightUp = true;
        m_StraightUpCoroutine = null;
    }
}

