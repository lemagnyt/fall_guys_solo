using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] float m_Speed;
    [SerializeField] Vector3 deltaPosition;
    [SerializeField] float m_StopTime;
    Vector3 m_StartPosition;
    Vector3 m_EndPosition;
    Coroutine m_MoveCoroutine;

    void Awake()
    {
        m_StartPosition = transform.position;
        m_EndPosition = m_StartPosition + deltaPosition;
        m_MoveCoroutine = null;
    }

    IEnumerator MoveObject(float speed, Vector3 startPosition, Vector3 endPosition)
    {
        while (true)
        {
            yield return StartCoroutine(MyTools.TranslationCoroutine(transform, startPosition, endPosition, speed));
            yield return new WaitForSeconds(m_StopTime);
            yield return StartCoroutine(MyTools.TranslationCoroutine(transform, endPosition, startPosition, speed));
            yield return new WaitForSeconds(m_StopTime);
        }
    }

    private void OnEnable()
    {
        m_MoveCoroutine = StartCoroutine(MoveObject(m_Speed, m_StartPosition, m_EndPosition));
    }


    private void OnDisable()
    {
        if (m_MoveCoroutine != null) StopCoroutine(m_MoveCoroutine);
    }

}
