using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWay : MonoBehaviour
{
    int m_Lines;
    int m_Columns;

    private void Awake()
    {
        m_Lines = transform.childCount;
        m_Columns = transform.GetChild(0).childCount;
        int line = 0;
        int column = GetRandomElement(m_Columns);
        transform.GetChild(line).GetChild(column).GetComponent<BoxCollider>().isTrigger = false;
        //transform.GetChild(line).GetChild(column).GetComponent<MeshRenderer>().material.color = Color.red;
        List<Vector2> nextInd;
        int nInd;
        int randInd;
        Vector2 newPos;
        while (line < m_Lines-1)
        {
            nextInd = new List<Vector2>();
            nextInd.Add(new Vector2(line + 1, column));
            if (line > 0 && column + 1 < m_Columns && GetTrigger(transform.GetChild(line).GetChild(column+1).gameObject))
            {
                if(GetTrigger(transform.GetChild(line-1).GetChild(column + 1).gameObject))
                    nextInd.Add(new Vector2(line, column + 1));
            }
            if (line > 0 && column - 1 >= 0 && GetTrigger(transform.GetChild(line).GetChild(column - 1).gameObject)) 
            {
                if (GetTrigger(transform.GetChild(line - 1).GetChild(column - 1).gameObject))
                    nextInd.Add(new Vector2(line, column - 1));
            }
            nInd = nextInd.Count;
            randInd = GetRandomElement(nInd);
            newPos = nextInd[randInd]; 
            line = (int)newPos.x;
            column = (int)newPos.y;
            transform.GetChild(line).GetChild(column).GetComponent<BoxCollider>().isTrigger = false;
            //transform.GetChild(line).GetChild(column).GetComponent<MeshRenderer>().material.color = Color.red;            
        }
    }

    private bool GetTrigger(GameObject platform)
    {
        return platform.GetComponent<BoxCollider>().isTrigger;
    }

    private int GetRandomElement(int number)
    {
        int randomIndex = Random.Range(0, number);
        return randomIndex;
    }
}
