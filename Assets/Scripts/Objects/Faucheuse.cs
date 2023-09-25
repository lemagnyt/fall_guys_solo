using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class Faucheuse : MonoBehaviour
{
    [SerializeField] public float m_StartSpeed;
    [SerializeField] public float m_DeltaSpeed;
    float m_CurrentSpeed;
    private Rigidbody m_Rigidbody;
    Coroutine m_FaucheuseCoroutine;
    float m_Timer;
    float m_Time;
    int m_SpeedMode;

    private void Awake()
    {
        m_CurrentSpeed = 0f;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Timer = LevelManager.maps[LevelManager.maps.Count-1].time;
        m_FaucheuseCoroutine = null;
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<FinishBeginTimerEvent>(StartGame);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<FinishBeginTimerEvent>(StartGame);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        StopFaucheuse();
        UnsubscribeEvents();
    }


    void StartGame(FinishBeginTimerEvent e)
    {
        m_FaucheuseCoroutine = StartCoroutine(StartFaucheuse(m_Timer));
    }

    IEnumerator StartFaucheuse(float timer)
    {
        m_CurrentSpeed = m_StartSpeed;
        while (m_Time < m_Timer)
        {
            yield return new WaitForSeconds(0.01f);
            m_Time += 0.01f;
            if (m_Time >= (m_SpeedMode + 1) * timer / 3)
            {
                m_SpeedMode++;
                m_CurrentSpeed += m_DeltaSpeed;
            }
        }
        StopCoroutine(m_FaucheuseCoroutine);
    }

    void StopFaucheuse()
    {
        if (m_FaucheuseCoroutine != null) StopCoroutine(m_FaucheuseCoroutine);
    }
    private void FixedUpdate()
    {
        // Obtient l'angle de rotation actuel
        Quaternion currentRotation = m_Rigidbody.rotation;
        float newRotationSpeed = m_CurrentSpeed * Time.fixedDeltaTime;
        // Calcule la rotation souhaitée pour cet instant
        Quaternion desiredRotation = Quaternion.AngleAxis(newRotationSpeed, transform.up)*currentRotation;
        // Applique la rotation au rigidbody
        m_Rigidbody.MoveRotation(desiredRotation);
    }
}
