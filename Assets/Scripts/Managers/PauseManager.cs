using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PauseManager : MonoBehaviour, IEventHandler
{
    [SerializeField] GameObject m_PausePanel;
    [SerializeField] GameObject m_Player;
    MotionController m_MotionScript;
    bool m_CanPause;
    bool m_GameStarted;

    // Start is called before the first frame update
    void Awake()
    {
        m_GameStarted = false;
        m_MotionScript = m_Player.GetComponent<MotionController>();
        m_CanPause = true;
    }

    void FixedUpdate()
    {
        bool escapePressed = Input.GetKey(KeyCode.Escape);
        if (m_GameStarted && escapePressed && m_CanPause)
        {
            m_CanPause = false;
            EventManager.Instance.Raise(new PauseButtonClickedEvent());
        }
        else if (!m_CanPause && !escapePressed) m_CanPause = true;
    }

    // Update is called once per frame
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GamePauseEvent>(PauseMenu);
        EventManager.Instance.AddListener<FinishBeginTimerEvent>(FinishBeginTimer);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GamePauseEvent>(PauseMenu);
        EventManager.Instance.RemoveListener<FinishBeginTimerEvent>(FinishBeginTimer);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    void PauseMenu(GamePauseEvent e)
    {
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
        if (BgMusic.m_BgInstance.m_Audio.isPlaying) BgMusic.m_BgInstance.m_Audio.Pause();
        else BgMusic.m_BgInstance.m_Audio.Play();
        m_PausePanel.SetActive(!m_PausePanel.activeSelf);
        m_MotionScript.IsPause = !m_MotionScript.IsPause;
    }

    public void PauseQuitButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new PauseQuitButtonClickedEvent());
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
        BgMusic.m_BgInstance.m_Audio.clip = BgMusic.m_BgInstance.m_MainMenu;
        BgMusic.m_BgInstance.m_Audio.Play();
    }
    public void FinishBeginTimer(FinishBeginTimerEvent e)
    {
        m_GameStarted = true;
    }
}

