using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingKinematics : MonoBehaviour
{
    private Animator m_Animator;
    private Coroutine m_ClimbCoroutine;
    private Rigidbody m_Rigidbody;
    private MainManager m_MainScript;
    private MotionController m_MotionScript;
    private bool isClimbing;
    private GameObject handleObject;
    public bool IsClimbing{
        get { return isClimbing; }
        set { isClimbing = value; }
    }
    public GameObject HandleObject{
        get { return handleObject; }
        set { handleObject = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        handleObject = null;
        isClimbing = false; 
        m_MainScript = GetComponent<MainManager>();
        m_MotionScript = GetComponent<MotionController>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
    }
    IEnumerator StartClimbing()
    {
        if(!isClimbing)isClimbing = true;
        Transform transformParent = handleObject.transform.parent;
        float groundAngle = Vector3.Angle(m_MainScript.m_GroundNormal.normalized,transform.up);
        float cosAngle = Mathf.Cos(Mathf.Deg2Rad * groundAngle);
        if(cosAngle!=0){
            float upMissing = (transform.lossyScale.y/2+transformParent.lossyScale.y/2 + (transformParent.position.y-transform.position.y))/cosAngle;
            Vector3 startPosition = transform.position;

            Vector3 position1 = startPosition;
            position1.y = transformParent.position.y + (transformParent.lossyScale.y*(3f/0.78f))/2 + 0.1f;
            Vector3 position2 = position1 + transform.forward.normalized;

            // Coroutine 1
            yield return StartCoroutine(MyTools.TranslationCoroutine(transform,startPosition,position1,4f));
            // Coroutine 2
            yield return StartCoroutine(MyTools.TranslationCoroutine(transform,position1,position2,4f));
            yield return new WaitForSeconds(0.8f);
        }
        // À la fin de la deuxième coroutine, on met isClimbing à true
        isClimbing = false;
        handleObject = null;
        m_ClimbCoroutine=null;
    }

    private void OnEnable(){
        m_Animator.SetBool("isClimbing",true);
        handleObject = m_MotionScript.HandleObject;
        m_MotionScript.HandleObject = null;
        m_ClimbCoroutine=StartCoroutine(StartClimbing());

    }

    private void OnDisable(){
        m_Animator.SetBool("isClimbing",false);
        m_Rigidbody.useGravity = true;
        isClimbing = false;
        handleObject = null;
        if(m_ClimbCoroutine!=null){
            StopCoroutine(m_ClimbCoroutine);
            m_ClimbCoroutine=null;
        }
    }
}