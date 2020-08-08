using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System;
using TMPro;

public class MigrationManager : MonoBehaviourPunCallbacks
{
    public static MigrationManager Instance { get; private set; } = null;

    public GameObject kickObject;
    public TMP_Text kickText;
    public TMP_Text disconnectText;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    private IEnumerator KickPlayer()
    {
        disconnectText.text = "Player Disconnected - You Win!";

        int count = 4;
        for (int i = 0; i < 5; i++)
        {
            kickText.text = (count + " Seconds").ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        PhotonNetwork.LeaveRoom();
        LevelLoader.Instance.LoadNextScene(2);
        Music.Instance.PlayTitleMusic();
        if (Music.Instance.skipToMain)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        StartCoroutine(GameManager.Instance.SetInstructionsText(""));
        kickObject.SetActive(true);
        StartCoroutine(KickPlayer());
    }
}