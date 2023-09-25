using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource m_Audio;

    public AudioClip m_Click;
    public AudioClip m_Hit;
    public AudioClip m_Jump;
    public AudioClip m_Dive;
    public AudioClip m_Grab;
    public AudioClip m_GrabOff;
    public AudioClip m_Woohoo;
    public AudioClip m_Woohoo2;
    public AudioClip m_Sad;
    public AudioClip m_Dead;
    public AudioClip m_OpenDoor;
    public AudioClip m_DrawingBlockOff;
    public AudioClip m_DrawingBlockOn;
    public AudioClip m_BoostSpeed;


    public AudioClip m_SelectMap;
    public AudioClip m_CountDown;
    public AudioClip m_Qualified;
    public AudioClip m_Eliminated;
    

    public static SFXManager m_SFXInstance;

    private void Awake()
    {
        m_SFXInstance = this;
    }
}
