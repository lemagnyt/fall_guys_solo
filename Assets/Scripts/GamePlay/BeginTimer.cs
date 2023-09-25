using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class BeginTimer : MonoBehaviour
{
    [SerializeField] int m_Timer;
    [SerializeField] Text m_Commentary;
    Text m_Text;
    int m_Time;
    Coroutine m_TimerCoroutine;

    void Awake()
    {
        m_TimerCoroutine = null;
        m_Time = 0;
        m_Text = GetComponent<Text>();
        m_Text.text = ((int)m_Timer).ToString();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<StartBeginTimerEvent>(StartBeginTimer);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<StartBeginTimerEvent>(StartBeginTimer);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        if (m_TimerCoroutine != null) StopCoroutine(m_TimerCoroutine);
        UnsubscribeEvents();
    }
    void StartBeginTimer(StartBeginTimerEvent e)
    {
        m_Text.enabled = true;
        m_Commentary.enabled = true;
        m_TimerCoroutine = StartCoroutine(Timer(m_Timer));
    }
    private IEnumerator Timer(float time)
    {       
        while (m_Time < m_Timer)
        {
            yield return new WaitForSeconds(1);
            m_Time ++;
            int timeLeft = m_Timer - m_Time;
            m_Text.text = timeLeft.ToString();
            if(timeLeft==3) SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_CountDown);
        }
        m_Text.enabled=false;
        m_Commentary.enabled = false;
        m_Time = 0;
        EventManager.Instance.Raise(new FinishBeginTimerEvent());

    }
}
