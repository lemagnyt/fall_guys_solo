using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
public class SkinManager : MonoBehaviour
{
    bool tailVisible;
    Color bodyColor;
    Color tailColor;
    private void Awake()
    {
        tailVisible = true;
    }
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ChangeTailEvent>(ChangeTail);
        EventManager.Instance.AddListener<ChangeTailColorEvent>(ChangeTailColor);
        EventManager.Instance.AddListener<ChangeBodyColorEvent>(ChangeBodyColor);
        EventManager.Instance.AddListener<GetTailEvent>(GiveTail);
        EventManager.Instance.AddListener<GetBodyColorEvent>(GiveBodyColor);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ChangeTailEvent>(ChangeTail);
        EventManager.Instance.RemoveListener<ChangeTailColorEvent>(ChangeTailColor);
        EventManager.Instance.RemoveListener<ChangeTailEvent>(ChangeTail);
        EventManager.Instance.RemoveListener<GetTailEvent>(GiveTail);
        EventManager.Instance.RemoveListener<GetBodyColorEvent>(GiveBodyColor);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    void ChangeTail(ChangeTailEvent e)
    {
        tailVisible = !tailVisible;
        EventManager.Instance.Raise(new GetTailEvent());
    }

    void ChangeBodyColor(ChangeBodyColorEvent e)
    {
        bodyColor = e.color;
        EventManager.Instance.Raise(new GetBodyColorEvent());
    }

    void ChangeTailColor(ChangeTailColorEvent e)
    {
        tailColor = e.color;
        EventManager.Instance.Raise(new GetTailEvent());
    }

    void GiveTail(GetTailEvent e)
    {
        EventManager.Instance.Raise(new GiveTailEvent(tailVisible,tailColor));
    }

    void GiveBodyColor(GetBodyColorEvent e)
    {
        EventManager.Instance.Raise(new GiveBodyColorEvent(bodyColor));
    }

    void GetBodyColor(GetBodyColorEvent e)
    {
        EventManager.Instance.Raise(new GiveBodyColorEvent(bodyColor));
    }

}
