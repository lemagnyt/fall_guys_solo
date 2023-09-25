using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDoor : MonoBehaviour
{
    [SerializeField] List<GameObject> m_Doors;
    private void Awake()
    {
        int nDoors = m_Doors.Count;
        int randInt = GetRandomElement(nDoors);
        m_Doors[randInt].GetComponent<BoxCollider>().isTrigger = true;
    }

    private int GetRandomElement(int number)
    {
        int randomIndex = Random.Range(0, number);
        return randomIndex;
    }
}

