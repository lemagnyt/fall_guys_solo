using UnityEngine;
using UnityEngine.UI;
using System;
[Serializable]
public class Map
{
    public string name;
    public Sprite image;
    public bool canRespawn;
    public float time;
    public int mode;
    public int scene;
    public AudioClip music;
    public bool select;

    // Constructeur de la classe Map
    public Map(string name, Sprite image, bool canRespawn, float time, int mode, int scene, AudioClip music)
    {
        this.name = name;
        this.image = image;
        this.canRespawn = canRespawn;
        this.time = time;
        this.mode = mode;
        this.scene = scene;
        this.music = music;
        this.select = false;
    }
}