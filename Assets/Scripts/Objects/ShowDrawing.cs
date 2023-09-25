using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class ShowDrawing : MonoBehaviour
{
    Text m_Text;
    Coroutine ShowCoroutine;
    void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ShowDrawingEvent>(ShowingDrawing);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ShowDrawingEvent>(ShowingDrawing);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
        if (ShowCoroutine != null)
        {
            StopCoroutine(ShowCoroutine);
            ShowCoroutine = null;
        }
    }

    IEnumerator ShowText(int drawing, int drawingMax)
    {
        m_Text.enabled = true;
        m_Text.text = "NEXT DRAWING\n" + (drawing+1) + " / " + drawingMax;
        yield return new WaitForSeconds(3f);
        m_Text.enabled = false;
        ShowCoroutine = null;
    }
    void ShowingDrawing(ShowDrawingEvent e)
    {
        ShowCoroutine = StartCoroutine(ShowText(e.drawing, e.drawingMax));
    }
}