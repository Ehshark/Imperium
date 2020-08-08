using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundController : MonoBehaviour
{
    public void Start()
    {
        Music.Instance.PlayTitleMusic();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!Music.Instance.skipToMain)
                LevelLoader.Instance.LoadNextScene(1);
            else
                LevelLoader.Instance.LoadNextScene(2);
        }
    }
}
