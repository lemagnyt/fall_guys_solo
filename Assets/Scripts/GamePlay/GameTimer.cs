using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class GameTimer : MonoBehaviour
{
    float m_Timer;
    Text m_Text;
    float m_Time;
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
        EventManager.Instance.AddListener<StartGameTimerEvent>(StartGameTimer);
        EventManager.Instance.AddListener<GetScoreEvent>(GiveScore);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<StartGameTimerEvent>(StartGameTimer);
        EventManager.Instance.RemoveListener<GetScoreEvent>(GiveScore);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        StopTimer();
        UnsubscribeEvents();
    }
    void StopTimer()
    {
        if (m_TimerCoroutine != null) StopCoroutine(m_TimerCoroutine);
    }
    void StartGameTimer(StartGameTimerEvent e)
    {
        m_Text.enabled = true;
        m_TimerCoroutine = StartCoroutine(Timer(e.timer));
    }

    void GiveScore(GetScoreEvent e)
    {
        StopTimer();
        EventManager.Instance.Raise(new GiveScoreEvent(m_Time));
    }
    private IEnumerator Timer(float timer)
    {
        while (m_Time < timer)
        {
            yield return new WaitForSeconds(0.01f);
            m_Time+=0.01f;
            float timeLeft = timer - m_Time;
            m_Text.text = timeLeft.ToString("F2");
        }
        m_Text.enabled = false;
        m_Time = 0;
        m_TimerCoroutine = null;
        EventManager.Instance.Raise(new FinishGameTimerEvent());
    }
}
