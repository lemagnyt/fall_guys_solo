using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

public class BestScore : MonoBehaviour
{

    Text m_Text;

    void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ShowBestScoreEvent>(ShowBestScore);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ShowBestScoreEvent>(ShowBestScore);
    }

    void ShowBestScore(ShowBestScoreEvent e)
    {
        string score1 = "No High Score !";
        string score2 = "No High Score !";
        string score3 = "No High Score !";
        string score4 = "No High Score !";

        if (PlayerPrefs.GetFloat("Obstacle 1") > 0) score1 = PlayerPrefs.GetFloat("Obstacle 1").ToString("F2") + " s";
        if (PlayerPrefs.GetFloat("Obstacle 2") > 0) score2 = PlayerPrefs.GetFloat("Obstacle 2").ToString("F2") + " s";
        if (PlayerPrefs.GetFloat("Drawing") > 0) score3 = PlayerPrefs.GetFloat("Drawing").ToString("F2") + " s";
        if (PlayerPrefs.GetFloat("Random Way") > 0) score4 = PlayerPrefs.GetFloat("Random Way").ToString("F2") + " s";
        m_Text.text = "Obstacle 1 : " + score1 + "\n" +
                 "Obstacle 2 : " + score2 + "\n" +
                 "Drawing : " + score3 + "\n" +
                 "Random Way : " + score4;
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