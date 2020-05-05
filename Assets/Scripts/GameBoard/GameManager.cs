using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Player
    public static string UserName { get; set; }
    public static Player player { get; set; } = null;

    public Transform alliedMinionZone;
    public Transform instructionsObj;
    public Transform enemyMinionZone;
    public bool canPlayCards = true;

    public static GameManager Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CleanUpListeners()
    {
        Destroy(enemyMinionZone.gameObject.GetComponent<DestroyMinionListener>());
    }
}
