using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource gameboardMusic;
    public AudioSource titleMusic;

    public static Music Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void PlayTitleMusic()
    {
        titleMusic.Play();
        gameboardMusic.Stop();
    }

    public void PlayGameboardMusic()
    {
        gameboardMusic.Play();
        titleMusic.Stop();
    }
}
