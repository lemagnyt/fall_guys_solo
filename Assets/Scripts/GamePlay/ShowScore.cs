using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class ShowScore : MonoBehaviour
{
    Text m_Text;
    void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ShowTextScoreEvent>(ShowingScore);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ShowTextScoreEvent>(ShowingScore);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
    void ShowingScore(ShowTextScoreEvent e)
    {
        if (LevelManager.maps.Count > 0 && LevelManager.scores.Count>0)
        {
            Map activeMap = LevelManager.maps[LevelManager.maps.Count-1];
            string score = LevelManager.scores[LevelManager.scores.Count-1];
            int mode = activeMap.mode;
            if (mode == 1) m_Text.text = "You finished the map in : " + score + " seconds !";
            else if (mode == 0) m_Text.text = "You survived during " + activeMap.time + " seconds !";
        } 
    }
}

