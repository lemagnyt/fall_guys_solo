using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour, IEventHandler
{
    [SerializeField] GameObject m_StartMenuPanel;
    [SerializeField] GameObject m_PlayMenuPanel;
    [SerializeField] GameObject m_MapMenuPanel;
    [SerializeField] GameObject m_WinMenuPanel;
    [SerializeField] GameObject m_LoseMenuPanel;
    [SerializeField] GameObject m_CreditMenuPanel;
    [SerializeField] GameObject m_CreditText;
    [SerializeField] GameObject m_ScoreLevelPanel;
    [SerializeField] Toggle m_ToggleSelectMap;
    [SerializeField] GameObject m_WarningMessage;
    [SerializeField] GameObject m_HighScorePannel;

    List<GameObject> m_Panels;

    void OpenPanel(GameObject panel)
    {
        m_Panels.ForEach(item => item.SetActive(panel == item));
    }


    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<StartMenuEvent>(StartMenu);
        EventManager.Instance.AddListener<PlayMenuEvent>(PlayMenu);
        EventManager.Instance.AddListener<GamePlayEvent>(GamePlay);
        EventManager.Instance.AddListener<SelectMapEvent>(SelectMap);
        EventManager.Instance.AddListener<WinMenuEvent>(WinMenu);
        EventManager.Instance.AddListener<LoseMenuEvent>(LoseMenu);
        EventManager.Instance.AddListener<ScoreLevelMenuEvent>(ScoreLevelMenu);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<StartMenuEvent>(StartMenu);
        EventManager.Instance.RemoveListener<PlayMenuEvent>(PlayMenu);
        EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
        EventManager.Instance.RemoveListener<SelectMapEvent>(SelectMap);
        EventManager.Instance.RemoveListener<WinMenuEvent>(WinMenu);
        EventManager.Instance.RemoveListener<LoseMenuEvent>(LoseMenu);
        EventManager.Instance.RemoveListener<ScoreLevelMenuEvent>(ScoreLevelMenu);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void Awake()
    {
        m_Panels = new List<GameObject>(
            new GameObject[] { m_StartMenuPanel, m_PlayMenuPanel, m_MapMenuPanel,
                m_WinMenuPanel, m_ScoreLevelPanel,m_LoseMenuPanel, m_CreditMenuPanel, m_HighScorePannel});
    }

    // GameManager events' callbacks
    void PlayMenu(PlayMenuEvent e)
    {
        OpenPanel(m_PlayMenuPanel);
    }

    void StartMenu(StartMenuEvent e)
    {
        OpenPanel(m_StartMenuPanel);
    }

    void GamePlay(GamePlayEvent e)
    {
        OpenPanel(null);
    }

    void SelectMap(SelectMapEvent e)
    {
        OpenPanel(m_MapMenuPanel);
        BgMusic.m_BgInstance.m_Audio.clip = null;
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_SelectMap);
    }

    void ScoreLevelMenu(ScoreLevelMenuEvent e)
    {
        OpenPanel(m_ScoreLevelPanel);
        EventManager.Instance.Raise(new ShowTextScoreEvent());
        StartCoroutine(LoadNextLevel(3f));
    }

    void WinMenu(WinMenuEvent e)
    {
        BgMusic.m_BgInstance.m_Audio.clip = BgMusic.m_BgInstance.m_Win;
        BgMusic.m_BgInstance.m_Audio.Play();
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Woohoo);
        OpenPanel(m_WinMenuPanel);
        EventManager.Instance.Raise(new ShowTextScoreRecapEvent());
    }

    void LoseMenu(LoseMenuEvent e)
    {
        OpenPanel(m_LoseMenuPanel);
        BgMusic.m_BgInstance.m_Audio.clip = null;
        SFXManager.m_SFXInstance.m_Audio.Stop();
        BgMusic.m_BgInstance.m_Audio.PlayOneShot(BgMusic.m_BgInstance.m_Loose);
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Sad);
        EventManager.Instance.Raise(new ShowTextScoreRecapEvent());
    }

    // UI events' callbacks
    public void PlayButtonHasBeenClicked()
    {
        if (!m_ToggleSelectMap.isOn || m_ToggleSelectMap.GetComponent<SelectMapToggle>().m_CanPlay)
        {
            SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
            SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Woohoo2);
            EventManager.Instance.Raise(new PlayMenuButtonClickedEvent());
        }
        else
        {
            m_WarningMessage.SetActive(true);
        }
    }


    public void ShowScoreFinished()
    {
        EventManager.Instance.Raise(new ShowScoreFinishedEvent());
    }

    public void StartMenuButtonHasBeenClicked()
    {
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Woohoo);
        EventManager.Instance.Raise(new StartMenuButtonClickedEvent());
    }

    public void QuitButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new QuitButtonClickedEvent());
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
    }

    private IEnumerator LoadNextLevel(float time)
    {
        yield return new WaitForSeconds(time);

        // La scène est chargée, appeler la fonction spécifiée
        EventManager.Instance.Raise(new PlayMenuButtonClickedEvent());
    }

    public void JoinMenuButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new InitLevelsEvent());
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
        OpenPanel(m_PlayMenuPanel);
        BgMusic.m_BgInstance.m_Audio.clip = BgMusic.m_BgInstance.m_MainMenu;
        BgMusic.m_BgInstance.m_Audio.Play();
    }
    public void CreditButtonClicked()
    {
        StartCoroutine(StartCredit());
    }

    public void HighScoreButtonClicked()
    {
        OpenPanel(m_HighScorePannel);
        EventManager.Instance.Raise(new ShowBestScoreEvent());
    }

    public void QuitHighScoreButtonClicked()
    {
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Click);
        OpenPanel(m_PlayMenuPanel);
    }

    private IEnumerator StartCredit()
    {
        OpenPanel(m_CreditMenuPanel);
        Transform creditTransform = m_CreditText.transform;
        Vector3 startPosition = creditTransform.position;
        yield return StartCoroutine(MyTools.TranslationCoroutine(creditTransform, startPosition, startPosition + Vector3.up * 510f, 10f));
        m_CreditText.transform.position = startPosition;
        OpenPanel(m_PlayMenuPanel);
    }
}