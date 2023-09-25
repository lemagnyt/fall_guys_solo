using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using Cinemachine;

public class PlayerManager : MonoBehaviour, IEventHandler
{
    [SerializeField] CinemachineFreeLook m_Cam;
    [SerializeField] GameObject m_Player;
    MainManager m_MainScript;
    int m_Zone;
    [SerializeField] GameObject m_SpawnPoints; 
    List<Vector3> m_Spawns;

    // Start is called before the first frame update
    void Awake()
    {
        m_MainScript = m_Player.GetComponent<MainManager>();
        m_Spawns = new List<Vector3>();
        int nSpawns = m_SpawnPoints.transform.childCount;

        for (int i = 0; i < nSpawns; i++)
        {
            Vector3 spawnPosition = m_SpawnPoints.transform.GetChild(i).position;
            m_Spawns.Add(spawnPosition);
        }
        m_Zone = 0;
    }

    // Update is called once per frame
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<PlayerSpawnEvent>(Spawn);
        EventManager.Instance.AddListener<PlayerLevelStartEvent>(PlayerLevelStart);
        EventManager.Instance.AddListener<PlayerLevelEndEvent>(PlayerLevelEnd);
        EventManager.Instance.AddListener<NewCheckPointEvent>(NewCheckPoint);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<PlayerSpawnEvent>(Spawn);
        EventManager.Instance.RemoveListener<PlayerLevelStartEvent>(PlayerLevelStart);
        EventManager.Instance.RemoveListener<PlayerLevelEndEvent>(PlayerLevelEnd);
        EventManager.Instance.RemoveListener<NewCheckPointEvent>(NewCheckPoint);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    void Spawn(PlayerSpawnEvent e)
    {
        int randIndex = GetRandomElement(m_Spawns.Count);
        Vector3 spawnPosition = m_Spawns[randIndex];
        m_Player.transform.position = spawnPosition;
        Rigidbody playerRb = m_Player.GetComponent<Rigidbody>();
        Quaternion SpawnRotation = Quaternion.FromToRotation(transform.forward, Vector3.forward) * Quaternion.FromToRotation(transform.up, Vector3.up);
        m_Player.transform.rotation = Quaternion.identity;
        playerRb.AddTorque(-playerRb.angularVelocity, ForceMode.VelocityChange);
        playerRb.AddForce(-playerRb.velocity, ForceMode.VelocityChange);
        if(m_MainScript.enabled)m_MainScript.InitPlayer();
        m_Cam.PreviousStateIsValid = false;
        m_Cam.m_XAxis.Value = 0;
        m_Cam.m_YAxis.Value = 0.5f;
    }

    private int GetRandomElement(int number)
    {
        int randomIndex = Random.Range(0, number);
        return randomIndex;
    }

    void PlayerLevelStart(PlayerLevelStartEvent e)
    {
        m_MainScript.enabled = true;
    }

    void PlayerLevelEnd(PlayerLevelEndEvent e)
    {
        m_MainScript.enabled = false;
    }

    void NewCheckPoint(NewCheckPointEvent e)
    {
        if(e.zone != m_Zone)
        {
            m_Zone = e.zone;
            m_Spawns = e.spawns;
        }
    }

}

