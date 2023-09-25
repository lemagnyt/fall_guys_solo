using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    private Animator m_Animator;

    private List<GameObject> m_Grounds;
    private bool isGrounded;
    StraightUpKinematics m_StraightUpKinematics;
    MotionController m_MotionController;
    ClimbingKinematics m_ClimbingKinematics;
    public Vector3 m_GroundNormal;
    private bool m_IsFalling;
    private float m_FallHitTimeMax;
    private float m_FallTime;
    private bool m_WasHit;
    public bool m_TouchGround;
    private float m_GroundAngle;
    [SerializeField] LayerMask m_HitLayer;
    [SerializeField] LayerMask m_GroundLayer;
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
    public bool TouchGround
    {
        get { return m_TouchGround; }
        set { m_TouchGround = value; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if((m_HitLayer.value & (1<<collision.gameObject.layer))!=0){
            float collisionForce = collision.impulse.magnitude;
            if(m_IsFalling){
                m_FallTime = 0f;
            }
            if(collisionForce>0.5){
                m_Animator.SetBool("isFalling",true);
                if(m_MotionController.enabled)m_MotionController.enabled = false;
                if(m_StraightUpKinematics.enabled)m_StraightUpKinematics.enabled = false;
                if(m_ClimbingKinematics.enabled)m_ClimbingKinematics.enabled = false;
                if(!m_IsFalling){
                    m_IsFalling=true;
                    m_Animator.SetBool("isFalling", true);
                    if(!m_WasHit) SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Hit);
                    m_WasHit = true;
                    m_FallTime = 0f;
                }
            }
            
        }
        if((m_GroundLayer.value & (1<<collision.gameObject.layer))!=0){
            if (!m_TouchGround)
            {
                m_TouchGround = true;
            }
            m_Grounds.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision){
        if((m_GroundLayer.value & (1 << collision.gameObject.layer)) != 0) { 
            m_Grounds.Remove(collision.gameObject);
            if (m_Grounds.Count <=0)m_TouchGround = false;
        }
    }

    void Awake()
    {
        m_GroundAngle = 0f;
        m_Grounds = new List<GameObject>();
        m_GroundNormal = Vector3.up;
        m_FallHitTimeMax = 1.5f;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_StraightUpKinematics = GetComponent<StraightUpKinematics>();
        m_MotionController = GetComponent<MotionController>();
        m_ClimbingKinematics = GetComponent<ClimbingKinematics>();
        m_Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 0.3f, m_GroundLayer))
        {
            m_GroundNormal = hit.normal;
            m_GroundAngle = Vector3.Angle(m_GroundNormal, transform.up);
            // Faites ici ce que vous souhaitez avec l'angle de collision
        }
        if(m_TouchGround)m_GroundAngle = Vector3.Angle(m_GroundNormal, transform.up);
        if (m_MotionController.enabled && m_MotionController.IsClimbing){
            m_MotionController.enabled = false;
            m_ClimbingKinematics.enabled = true;
        }
        else if(m_ClimbingKinematics.enabled && !m_ClimbingKinematics.IsClimbing){
            m_MotionController.enabled = true;
            m_ClimbingKinematics.enabled = false;
        }
        if(!m_IsFalling && m_MotionController.enabled && m_MotionController.HasDive)
        {
            m_MotionController.enabled = false;
            m_Animator.SetBool("isFalling",true);
            m_IsFalling = true;
            m_FallTime = 0f;
        }
        if (m_IsFalling)
        {
            m_FallTime += Time.deltaTime;
            if(m_TouchGround){
                if(((m_WasHit || m_MotionController.HasDive) && m_FallTime >= m_FallHitTimeMax) || m_GroundAngle >= 80){
                    if(m_WasHit)m_WasHit=false;
                    if (m_MotionController.HasDive) m_MotionController.HasDive = false;
                     m_IsFalling = false;
                    m_Animator.SetBool("isFalling", false);
                    m_StraightUpKinematics.enabled = true;
                }
                //To evite bugs when you are stuck
                else if(m_FallTime>5f)
                {
                    m_Animator.SetBool("isFalling",false);
                    transform.position += m_GroundNormal*2f;
                    m_Rigidbody.MoveRotation(Quaternion.FromToRotation(transform.up,m_GroundNormal));
                    m_IsFalling = false;
                    m_MotionController.enabled = true;
                    if(m_WasHit)m_WasHit=false;
                    if (m_MotionController.HasDive) m_MotionController.HasDive = false;
                }
            }
        }
        else if(m_StraightUpKinematics.enabled && m_StraightUpKinematics.IsStraightUp)
        {
            m_StraightUpKinematics.enabled = false;
            m_MotionController.enabled = true;
        }      
    }

    void OnEnable()
    {
        m_MotionController.enabled = true;
        m_Rigidbody.useGravity = true;
    }

    void OnDisable()
    {
        InitPlayer();
    }

    public void InitPlayer()
    {
        m_MotionController.enabled = false;
        m_MotionController.enabled = true;
        m_StraightUpKinematics.enabled = false;
        m_ClimbingKinematics.enabled = false;
        m_IsFalling = false;
        m_WasHit = false;
        m_TouchGround = false;
        m_Animator.SetBool("isFalling", false);
    }
   

}
