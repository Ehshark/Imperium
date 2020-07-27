using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; } = null;
    public Animator transition;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoadNextScene(int buildIndex)
    {
        StartCoroutine(LoadScene(buildIndex));
    }

    private IEnumerator LoadScene(int buildIndex)
    {
        transition.SetTrigger("animate");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(buildIndex);
    }
}
