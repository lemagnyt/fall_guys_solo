using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

public class MapManager : MonoBehaviour, IEventHandler
{
    [SerializeField] List<Map> m_Maps;
    bool m_SelectMode;
    int m_SelectCount;
   
    void Awake()
    {
    }

    void SelectMap(SelectMapEvent e)
    {
        StartCoroutine(StartSelectMap(m_Maps));
    }

    IEnumerator StartSelectMap(List<Map> maps)
    {
        foreach(Map map in maps)
        {
        }
        Map activeMap = null;
        int mapCount = m_Maps.Count;
        if (m_SelectMode && m_SelectCount>0) mapCount = m_SelectCount;
        if (LevelManager.maps.Count == 0) EventManager.Instance.Raise(new GiveMapCountEvent(mapCount));
        else activeMap = LevelManager.maps[LevelManager.maps.Count - 1];
        Map randomMap = null;
        int totalIterations = (mapCount-LevelManager.maps.Count) * 2;
        if (totalIterations == 2) totalIterations = 1;
        int currentIteration = 0;
        int previousRandIndex = -1;
        yield return new WaitForSeconds(0.1f);
        while (currentIteration < totalIterations)
        {
            int randIndex = GetRandomElement(maps.Count);
            randomMap = maps[randIndex];
            if (previousRandIndex != randIndex && activeMap != randomMap && !LevelManager.maps.Contains(randomMap))
            {
                if (!m_SelectMode || m_SelectCount==0 || randomMap.select)
                {
                    previousRandIndex = randIndex;
                    EventManager.Instance.Raise(new ChangeMapNameEvent(randomMap.name));
                    EventManager.Instance.Raise(new ChangeMapImageEvent(randomMap.image));
                    currentIteration++;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            // Attendre pendant 0.5 seconde
        }
        yield return new WaitForSeconds(5f);
        LevelManager.maps.Add(randomMap);
        BgMusic.m_BgInstance.m_Audio.clip = randomMap.music;
        EventManager.Instance.Raise(new HasSelectedMapEvent(randomMap.scene));

    }

    private int GetRandomElement(int number)
    {
        int randomIndex = Random.Range(0, number);
        return randomIndex;
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<SelectMapEvent>(SelectMap);
        EventManager.Instance.AddListener<SelectModeEvent>(SelectMode);
        EventManager.Instance.AddListener<AddSelectMapEvent>(AddSelectMap);
        EventManager.Instance.AddListener<RemoveSelectMapEvent>(RemoveSelectMap);
        EventManager.Instance.AddListener<InitLevelsEvent>(InitLevels);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<SelectMapEvent>(SelectMap);
        EventManager.Instance.RemoveListener<SelectModeEvent>(SelectMode);
        EventManager.Instance.RemoveListener<AddSelectMapEvent>(AddSelectMap);
        EventManager.Instance.RemoveListener<RemoveSelectMapEvent>(RemoveSelectMap);
        EventManager.Instance.RemoveListener<InitLevelsEvent>(InitLevels);
    }

    void SelectMode(SelectModeEvent e)
    {
        if (m_SelectMode)
        {
            InitSelectMap();
        }
        else m_SelectMode = true;

    }

    void InitSelectMap()
    {
        m_SelectCount = 0;
        EventManager.Instance.Raise(new InitSelectMapEvent());
        m_SelectMode = false;
    }

    void InitLevels(InitLevelsEvent e)
    {
        foreach(Map map in m_Maps)
        {
            map.select = false;
        }
        m_SelectCount = 0;
        m_SelectMode = false;
    }

    void AddSelectMap(AddSelectMapEvent e)
    {
        m_Maps[e.mapInd].select = true;
        m_SelectCount++;
    }

    void RemoveSelectMap(RemoveSelectMapEvent e)
    {
        m_Maps[e.mapInd].select = false;
        m_SelectCount--;
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
}
