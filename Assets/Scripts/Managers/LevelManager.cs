using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class LevelManager : MonoBehaviour, IEventHandler
{
    int nbLevel;
    int activeLevel;
    public static List<Map> maps;
    public static List<string> scores;

    // Start is called before the first frame update
    void Awake()
    {
        scores = new List<string>();
        maps = new List<Map>();
        activeLevel = 0;
    }

    // Update is called once per frame
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<FinishLevelEvent>(FinishLevel);
        EventManager.Instance.AddListener<FinishGameTimerEvent>(FinishGameTimer);
        EventManager.Instance.AddListener<StartLevelEvent>(StartLevel);
        EventManager.Instance.AddListener<FinishBeginTimerEvent>(Play);
        EventManager.Instance.AddListener<InitLevelsEvent>(InitLevels);
        EventManager.Instance.AddListener<DeadLevelEvent>(DeadLevel);
        EventManager.Instance.AddListener<GiveScoreEvent>(GetScore);
        EventManager.Instance.AddListener<GiveMapCountEvent>(GetMapCount);

    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<FinishLevelEvent>(FinishLevel);
        EventManager.Instance.RemoveListener<FinishGameTimerEvent>(FinishGameTimer);
        EventManager.Instance.RemoveListener<StartLevelEvent>(StartLevel);
        EventManager.Instance.RemoveListener<FinishBeginTimerEvent>(Play);
        EventManager.Instance.RemoveListener<InitLevelsEvent>(InitLevels);
        EventManager.Instance.RemoveListener<DeadLevelEvent>(DeadLevel);
        EventManager.Instance.RemoveListener<GiveMapCountEvent>(GetMapCount);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    void GetMapCount(GiveMapCountEvent e)
    {
        nbLevel = e.count;
    }
    void GetScore(GiveScoreEvent e)
    {
        Map activeMap = maps[activeLevel - 1];
        if (activeMap.mode == 1)
        {
            float currentScoreValue = e.score;
            string mapName = activeMap.name;
            float highScoreValue = PlayerPrefs.GetFloat(mapName);
            if (highScoreValue > currentScoreValue || highScoreValue == 0f) PlayerPrefs.SetFloat(mapName, currentScoreValue);
            scores.Add(e.score.ToString("F2"));
        }
        else if (activeMap.mode == 0) scores.Add("No Score");
    }
    void FinishLevel(FinishLevelEvent e)
    {
        BgMusic.m_BgInstance.m_Audio.clip = null;
        EventManager.Instance.Raise(new GetScoreEvent());

        if (activeLevel == nbLevel)
        {
            EventManager.Instance.Raise(new WinLevelEvent());
        }
        else
        {
            SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Qualified);
            EventManager.Instance.Raise(new PlayerLevelEndEvent());
            EventManager.Instance.Raise(new ScoreLevelEvent());
        }

    }

    void FinishGameTimer(FinishGameTimerEvent e)
    {
        Map activeMap = maps[activeLevel - 1];
        if (activeMap.mode == 1) LoseLevel();
        else if (activeMap.mode == 0) EventManager.Instance.Raise(new FinishLevelEvent());
    }
    void StartLevel(StartLevelEvent e)
    {
        activeLevel++;
        string levelText;
        if (activeLevel == nbLevel) levelText = "FINAL LEVEL";
        else levelText = "LEVEL " + activeLevel.ToString();
        EventManager.Instance.Raise(new PlayerSpawnEvent());
        EventManager.Instance.Raise(new StartBeginTimerEvent());
        EventManager.Instance.Raise(new ShowLevelEvent(levelText));
    }

    void Play(FinishBeginTimerEvent e)
    {
        Map activeMap = maps[activeLevel - 1];
        EventManager.Instance.Raise(new StartGameTimerEvent(activeMap.time));
        EventManager.Instance.Raise(new PlayerLevelStartEvent());
        BgMusic.m_BgInstance.m_Audio.Play();
        SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Woohoo2);
    }

    void InitLevels(InitLevelsEvent e)
    {
        maps = new List<Map>();
        scores = new List<string>();
        activeLevel = 0;
        nbLevel = 0;
    }

    void DeadLevel(DeadLevelEvent e)
    {
        Map activeMap = maps[activeLevel - 1];
        if (activeMap.canRespawn) EventManager.Instance.Raise(new PlayerSpawnEvent());
        else LoseLevel();
    }
    void LoseLevel()
    {
        scores.Add("ELIMINATED");
        EventManager.Instance.Raise(new LoseLevelEvent());
    }
}