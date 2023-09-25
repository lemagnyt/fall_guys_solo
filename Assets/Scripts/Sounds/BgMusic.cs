using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusic : MonoBehaviour
{
    public AudioSource m_Audio;

    public AudioClip m_MainMenu;
    public AudioClip m_Win;
    public AudioClip m_Loose;

    public static BgMusic m_BgInstance;

    private void Awake()
    {
        m_BgInstance = this;
    }
}
