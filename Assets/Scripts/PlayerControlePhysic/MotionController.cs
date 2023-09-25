using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
public class MotionController : MonoBehaviour
{

    private float m_Boost;
    private Quaternion m_LocalAngle;
    private GameObject m_ParentObject;
    
    public Quaternion LocalAngle
    {
        get { return m_LocalAngle; }
        set { m_LocalAngle = value; }
    }

    public GameObject ParentObject
    {
        get { return m_ParentObject; }
        set { m_ParentObject = value; }
    }

    private Animator m_Animator;
    private int m_IsMovingHash;
    private bool m_IsPause;
    
    [SerializeField] private float m_TranslationSpeed;
    [SerializeField] private float m_GrabTranslationSpeed;
    [SerializeField] private float m_JumpTranslationSpeed;
    [SerializeField] private float m_PushTranslationSpeed;
    [SerializeField] private float m_RotationSpeed;
    private float m_TimeMinJump;
    float m_TimeDive;
    [SerializeField] float m_DeltaTimeDive;
    [SerializeField] float m_JumpForce;
    [SerializeField] float m_FallForce;
    [SerializeField] float m_DiveForce;
    private float m_TimeNextDive;
    [SerializeField] private float throwForce;
    Rigidbody m_Rigidbody;
    private MainManager m_MainScript;

    private bool hasDive;

    private bool m_IsJumping;
    private bool m_IsDiving;
    private bool m_CanDive;
    private bool m_IsPushing;
    private bool m_IsHandling;
    private bool isClimbing;
    private bool m_IsGrabing;
    private bool m_CanClimb;

    private bool canPush;
    private bool canHandle;
    private bool canGrab;

    private GameObject handleObject;
    private GameObject pushObject;
    private List<GameObject> m_GrabObjects;

    private float m_GrabTimer;

    public bool IsClimbing
    {
        get{ return isClimbing;}
        set{ isClimbing = value;}
    }
    public bool CanPush
    {
        get { return canPush; }
        set { canPush = value; }
    }
    public GameObject PushObject{
        get{ return pushObject; }
        set{ pushObject = value; }
    }

    public bool IsPushing{
        get{ return m_IsPushing; }
        set{ m_IsPushing = value; }
    }

    public bool IsHandling{
        get{ return m_IsHandling; }
        set{ m_IsHandling = value; }
    }
    public bool CanHandle
    {
        get { return canHandle; }
        set { canHandle = value; }
    }
    public GameObject HandleObject{
        get { return handleObject; }
        set { handleObject = value; }
    }

    public bool IsGrabing
    {
        get { return m_IsGrabing; }
        set { m_IsGrabing = value; }
    }
    public bool CanGrab
    {
        get { return canGrab; }
        set { canGrab = value; }
    }
    public List<GameObject> GrabObjects
    {
        get { return m_GrabObjects; }
        set { m_GrabObjects = value; }
    }

    public float GrabTimer
    {
        get { return m_GrabTimer; }
        set { m_GrabTimer = value; }
    }

    public bool HasDive
    {
        get { return hasDive; }
        set { hasDive = value; }
    }

    public bool IsPause
    {
        get { return m_IsPause; }
        set { m_IsPause = value; }
    }

    Coroutine SpeedCoroutine;

    void Awake()
    {
        m_GrabObjects = new List<GameObject>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_MainScript = GetComponent<MainManager>();
        m_TimeMinJump = Time.time;
        m_TimeDive = Time.time;
        m_TimeNextDive = Time.time;
        m_Animator = GetComponent<Animator>();
        m_IsMovingHash = Animator.StringToHash("isMoving");
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<SpeedPlayerEvent>(SpeedPlayer);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<SpeedPlayerEvent>(SpeedPlayer);
    }

    private void FixedUpdate()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        bool spacePressed = Input.GetKey("space");
        bool pushPressed = Input.GetKey("p");
        bool grabPressed = Input.GetKey("g");
        bool throwPressed = Input.GetKey("t");
        
        if(m_IsPause)
        {
            hInput =0;
            vInput = 0;
            spacePressed = false;
            pushPressed = false;
            grabPressed = false;
            throwPressed = false;
        }
        bool isMovingA = m_Animator.GetBool(m_IsMovingHash);

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 currentPosition = transform.position;
        Vector3 moveDirection = forward*vInput + right*hInput;
        Vector3 targetVelocity = moveDirection * m_TranslationSpeed;
        if (m_IsPushing) targetVelocity = moveDirection * m_PushTranslationSpeed;
        else if (m_IsJumping) targetVelocity = moveDirection * m_JumpTranslationSpeed;
        else if (m_IsGrabing) targetVelocity = moveDirection * m_GrabTranslationSpeed;
        Vector3 deltaVelocity = targetVelocity - new Vector3(m_Rigidbody.velocity.x,0,m_Rigidbody.velocity.z);


        if (!m_IsDiving && !m_IsHandling && !m_IsGrabing && !m_IsDiving && !HasDive
            && !m_IsPushing)
        {
            // ----------------------------------- POUSSER -------------------------------------- //
            if (canPush && pushPressed && m_MainScript.IsGrounded && !m_IsJumping)
            {
                StartPushing();
            }
            // ----------------------------------- AGRIPPER -------------------------------------- //
            else if (pushPressed && canHandle)
            {
                StartHandling();
            }
            // ----------------------------------- ATTRAPER UN OBJET ---------------------------------- //
            else if (grabPressed && canGrab && !m_IsJumping)
            {
                if (m_GrabTimer <= Time.time)
                {
                    m_GrabObjects[0].GetComponent<GrabObject>().IsGrabed();
                }
            }
        }

        // ------------------------------------- SAUT ---------------------------------------- //
        if (spacePressed && !m_IsJumping && !m_IsDiving && !m_IsPushing){
            if(!m_IsHandling && m_MainScript.IsGrounded){
                StartJumping();
            }
            else if (m_IsHandling && m_CanClimb) {
                isClimbing = true;
                StopHandling();
                m_Animator.SetBool("isClimbing", true);
                SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Dive);
                return;
            }
        }
        else if(m_IsJumping){
            if (m_Rigidbody.velocity.y < 0)
            {
                m_Rigidbody.AddForce(new Vector3(0f, -m_FallForce, 0), ForceMode.Force);
            }
            if (m_MainScript.IsGrounded  && Time.time > m_TimeMinJump){
                StopJumping();
            }
            else if(!spacePressed && !m_CanDive && !m_MainScript.IsGrounded  && !m_IsGrabing)m_CanDive=true;
        // ------------------------------------- PLONGEON ---------------------------------------- //
            else if(m_CanDive && spacePressed && !m_MainScript.IsGrounded && Time.time >= m_TimeNextDive){
                StartDiving();
            }
        }
        // ----------------------------------- DEPLACEMENT -------------------------------------- //
        if(!m_IsDiving && !hasDive && !m_IsHandling){
            Quaternion qUprightRot = Quaternion.FromToRotation(transform.up, Vector3.up);            
            Quaternion qUprightOrient = Quaternion.Slerp(
                            transform.rotation,
                            qUprightRot * transform.rotation,
                            Time.fixedDeltaTime * 8);
            m_Rigidbody.MoveRotation(qUprightOrient);
            if (m_ParentObject != null)
            {
                m_Rigidbody.rotation *= m_LocalAngle;
            }

            m_Rigidbody.AddTorque(-m_Rigidbody.angularVelocity,ForceMode.VelocityChange);
            m_Rigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);
            if(moveDirection.sqrMagnitude>0){
                if (!m_IsPushing)
                {
                    m_Rigidbody.MoveRotation(Quaternion.Lerp(m_Rigidbody.rotation,Quaternion.LookRotation(moveDirection),Time.fixedDeltaTime*7));
                }
                if(!isMovingA && !m_IsJumping)m_Animator.SetBool("isMoving",true);
            }
            else if (isMovingA) m_Animator.SetBool("isMoving", false);
        }
        if(m_IsDiving && Time.time > m_TimeDive && m_MainScript.TouchGround){
            StopDiving();
        }
        if((!pushPressed || !canPush) && m_IsPushing){
            StopPushing();
            SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_GrabOff);
        }
        if(m_IsHandling){
            if (!m_CanClimb && !spacePressed) m_CanClimb = true;
            if(!pushPressed || !canHandle){
                StopHandling();
                SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_GrabOff);
            } 
        }
        if (m_IsGrabing)
        {
            if (throwPressed)
            {
                m_GrabObjects[0].GetComponent<GrabObject>().Throw(throwForce);
            }
            if (!grabPressed)
            {
                m_GrabObjects[0].GetComponent<GrabObject>().IsNotGrabed();
                SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_GrabOff);
            }
        }
    }

    public void GrabTimerOn()
    {
        m_GrabTimer = Time.time + 2;
    }
    private void resetMotion(){
        if(m_IsDiving)StopDiving();
        if(m_IsJumping){
            StopJumping();
        }
        if(m_IsPushing){
            StopPushing();
        }
        canPush=false;
        canHandle=false;
        hasDive=false;
        m_CanDive=false;
        if (m_IsGrabing) m_GrabObjects[0].GetComponent<GrabObject>().IsNotGrabed();
        canGrab = false;
        m_GrabObjects = new List<GameObject>();
        if (m_IsHandling)StopHandling();

        pushObject = null;
        if (!isClimbing) handleObject = null;
        else isClimbing = false;
        m_GrabTimer = 0f;
    }

    private void SpeedPlayer(SpeedPlayerEvent e)
    {
        if (SpeedCoroutine != null)
        {
            StopCoroutine(SpeedCoroutine);
            if (m_Boost > 0)
            {
                m_GrabTranslationSpeed /= m_Boost;
                m_JumpTranslationSpeed /= m_Boost;
                m_PushTranslationSpeed /= m_Boost;
                m_TranslationSpeed /= m_Boost;
            }
        }
        m_Boost = e.boostSpeed;
        SpeedCoroutine = StartCoroutine(ChangeSpeed(e.time,e.boostSpeed));
    }

    private IEnumerator ChangeSpeed(float time, float boostSpeed)
    {
        m_GrabTranslationSpeed *= boostSpeed;
        m_JumpTranslationSpeed *= boostSpeed;
        m_PushTranslationSpeed *= boostSpeed;
        m_TranslationSpeed *= boostSpeed;
        yield return new WaitForSeconds(time);
        m_GrabTranslationSpeed /= boostSpeed;
        m_JumpTranslationSpeed /= boostSpeed;
        m_PushTranslationSpeed /= boostSpeed;
        m_TranslationSpeed /= boostSpeed;
        SpeedCoroutine = null;
        m_Boost = 0f;
    }

    private void OnDisable(){
        UnsubscribeEvents();
        resetMotion();
        if (SpeedCoroutine != null)
        {
            if (m_Boost > 0)
            {
                m_GrabTranslationSpeed /= m_Boost;
                m_JumpTranslationSpeed /= m_Boost;
                m_PushTranslationSpeed /= m_Boost;
                m_TranslationSpeed /= m_Boost;
            }
            SpeedCoroutine = null;
            m_Boost = 0f;
        }
        m_ParentObject = null;
        m_LocalAngle = Quaternion.identity;
    }

    private void OnEnable()
    {
        SubscribeEvents();
        m_TimeMinJump = Time.time + 1f;
    }

    #region Jump Movement
    private void StartJumping()
    {
        m_Rigidbody.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);
        m_IsJumping = true;
        m_TimeMinJump = Time.time + 0.5f;
        if(!m_IsGrabing)m_Animator.SetBool("isJumping", true);
        m_Animator.SetBool("isMoving", false);
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Jump);
    }
    private void StopJumping()
    {
        m_IsJumping = false;
        m_CanDive = false;
        m_Animator.SetBool("isJumping", false);
    }
    #endregion

    #region Dive Movement
    private void StartDiving()
    {
        m_Rigidbody.AddForce(-m_Rigidbody.velocity, ForceMode.VelocityChange);
        m_Rigidbody.AddForce(transform.forward * m_DiveForce, ForceMode.Impulse);
        Quaternion currentRotation = m_Rigidbody.rotation;
        Vector3 eulerAngles = currentRotation.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(70f, eulerAngles.y, eulerAngles.z); // crée un quaternion qui représente une rotation de 90 degrés autour de l'axe x, en conservant les valeurs actuelles pour les axes Y et Z
        m_Rigidbody.MoveRotation(targetRotation);
        m_IsDiving = true;
        StopJumping();
        m_TimeDive = Time.time + m_DeltaTimeDive;
        m_Animator.SetBool("isDiving", true); SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Dive);
    }
    private void StopDiving()
    {
        m_IsDiving = false;
        HasDive = true;
        //m_Rigidbody.AddForce(-m_Rigidbody.velocity, ForceMode.VelocityChange);
        m_Animator.SetBool("isDiving", false);
        m_TimeNextDive = Time.time + 1.5f;
    }
    #endregion

    #region Handle Movement
    private void StartHandling()
    {
        m_IsHandling = true;
        m_Animator.SetBool("isMoving", false);
        m_Animator.SetBool("isHandling", true);
        m_Rigidbody.useGravity = false;
        m_Rigidbody.AddForce(-m_Rigidbody.velocity, ForceMode.VelocityChange);
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, handleObject.transform.up) * transform.rotation;
        targetRotation = Quaternion.FromToRotation(transform.forward, -handleObject.transform.forward) * transform.rotation;
        m_Rigidbody.MoveRotation(targetRotation);
        StopJumping();
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Grab);
    }
    private void StopHandling()
    {
        m_Animator.SetBool("isHandling", false);
        m_IsHandling = false;
        if(!isClimbing)m_Rigidbody.useGravity = true;
        m_CanClimb = false;
    }
    #endregion

    #region Push Movement
    private void StartPushing()
    {
        m_IsPushing = true;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        PushObject.transform.SetParent(transform);
        m_Animator.SetBool("isPushing", true);
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Grab);
    }
    private void StopPushing()
    {
        pushObject.transform.SetParent(null);
        m_IsPushing = false;
        m_Animator.SetBool("isPushing", false);
        m_Rigidbody.constraints = RigidbodyConstraints.None;
    }
    #endregion

}
