using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class ShowScoreRecap : MonoBehaviour
{
    Text m_Text;
    void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ShowTextScoreRecapEvent>(ShowingScoreRecap);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ShowTextScoreRecapEvent>(ShowingScoreRecap);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }
    void ShowingScoreRecap(ShowTextScoreRecapEvent e)
    {
        int nMaps = LevelManager.maps.Count;
        if (nMaps>0)
        {
            string score = "";
            Map map;
            for(int i = 0; i<nMaps; i++)
            {
                map = LevelManager.maps[i];
                score += "Map " + (i + 1) + " : " + map.name + " -> ";
                if(i==nMaps-1 && LevelManager.scores[i]=="ELIMINATED") score += "ELIMINATED \n";
                else if (map.mode == 0) score += "SURVIVED \n";
                else if (map.mode == 1) score += "FINISHED IN " + LevelManager.scores[i] + " s\n";
            }
            m_Text.text = score;
        }
    }
}

