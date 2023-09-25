using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
public class DrawingManager : MonoBehaviour
{
    int m_NDrawing;
    [SerializeField] int m_NDrawingMax;
    [SerializeField] GameObject m_Model;
    [SerializeField] GameObject m_Drawing;
    List<int[,]> m_ModelDrawings;

    private void Awake()
    {
        EventManager.Instance.Raise(new InitDrawingEvent());
        int[,] model1 = { {1,0,0,0,0,0,0,1},
                          {1,1,0,0,0,0,1,1},
                          {1,1,1,0,0,1,1,1},
                          {1,1,1,1,1,1,1,1},
                          {1,1,1,1,1,1,1,1},
                          {1,1,1,0,0,1,1,1},
                          {1,1,0,0,0,0,1,1},
                          {1,0,0,0,0,0,0,1}};
        int[,] model2 = { {0,0,0,1,1,0,0,0},
                          {0,0,1,0,0,1,0,0},
                          {0,1,0,0,0,0,1,0},
                          {1,0,0,1,1,0,0,1},
                          {1,0,0,1,1,0,0,1},
                          {0,1,0,0,0,0,1,0},
                          {0,0,1,0,0,1,0,0},
                          {0,0,0,1,1,0,0,0}};
        int[,] model3 = {   { 1,1,0,0,0,1,1,0},
                            { 1,0,1,0,0,1,0,1},
                            { 1,0,0,1,0,1,0,1},
                            { 1,0,0,1,0,1,1,0},
                            { 1,0,0,1,0,1,0,1},
                            { 1,0,0,1,0,1,0,1},
                            { 1,0,1,0,0,1,0,1},
                            { 1,1,0,0,0,1,1,0}};
        int[,] smileyFace = {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0, 1, 1, 0 },
            { 0, 1, 1, 0, 0, 1, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 0, 1, 0 },
            { 0, 0, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 }};
        int[,] tree = {
            { 0, 0, 0, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 1, 1, 0, 0, 0 }};
        int[,] model4 = {
            { 0, 1, 0, 0, 0, 0, 1, 0 },
            { 0, 0, 1, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 1, 0, 0 },
            { 0, 1, 0, 0, 0, 0, 1, 0 }};
        int[,] heart = {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0, 1, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 }};
        m_ModelDrawings = new List<int[,]>();
        m_ModelDrawings.Add(model1);
        m_ModelDrawings.Add(model2);
        m_ModelDrawings.Add(model3);
        m_ModelDrawings.Add(model4);
        m_ModelDrawings.Add(heart);
        m_ModelDrawings.Add(smileyFace);
        m_ModelDrawings.Add(tree);
        ShuffleList(m_ModelDrawings);
        ChangeModel();
    }

    static bool Compare2DTable(int[,] table1, int[,] table2)
    {
        if (table1.GetLength(0) != table2.GetLength(0) || table1.GetLength(1) != table2.GetLength(1))
        {
            return false; // Les dimensions des tableaux sont différentes
        }

        for (int i = 0; i < table1.GetLength(0); i++)
        {
            for (int j = 0; j < table1.GetLength(1); j++)
            {
                if (table1[i, j] != table2[i, j])
                {
                    return false; // Les éléments des tableaux sont différents
                }
            }
        }

        return true; // Les tableaux sont identiques
    }

    int[,] DrawingToTable(GameObject drawing)
    {
        int[,] table = new int[8,8];
        GameObject bloc;
        int line = 0;
        for(int i = 0; i<8; i++)
        {
            line = 8 - 1 - i;
            for (int j = 0; j < 8; j++)
            {
                bloc = drawing.transform.GetChild(line).GetChild(j).gameObject;
                if (Color.yellow == bloc.GetComponent<MeshRenderer>().material.color)
                {
                    table[i, j] = 1;
                }                
            }
        }
        return table;
    }

    void DrawModel(int[,] model)
    {
        GameObject bloc;
        int line;
        for(int i = 0; i<model.GetLength(0); i++)
        {
            line = model.GetLength(0) - 1 - i;
            for (int j = 0; j < model.GetLength(1); j++)
            { 
                bloc = m_Model.transform.GetChild(line).GetChild(j).gameObject;
                if (model[i,j] == 1) {
                    bloc.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                else {
                    bloc.GetComponent<MeshRenderer>().material.color = Color.grey;
                }
            }
        }   
    }

    void ChangeModel()
    {
        DrawModel(m_ModelDrawings[m_NDrawing]);
    }

    void ChangeDrawing(ChangeDrawingEvent e)
    {
        int[,] tableDrawing = DrawingToTable(m_Drawing);
        if (Compare2DTable(tableDrawing, m_ModelDrawings[m_NDrawing])) FinishDrawing();
    }

    void FinishDrawing()
    {
        if (m_NDrawing == m_NDrawingMax - 1) EventManager.Instance.Raise(new FinishLevelEvent());
        else
        {
            NextDrawing();
            SFXManager.m_SFXInstance.m_Audio.PlayOneShot(SFXManager.m_SFXInstance.m_Woohoo);
        }
    }

    void NextDrawing()
    {
        m_NDrawing++;
        EventManager.Instance.Raise(new InitDrawingEvent());
        EventManager.Instance.Raise(new ShowDrawingEvent(m_NDrawing,m_NDrawingMax));
        EventManager.Instance.Raise(new PlayerSpawnEvent());
        ChangeModel();
    }

    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ChangeDrawingEvent>(ChangeDrawing);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ChangeDrawingEvent>(ChangeDrawing);
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
