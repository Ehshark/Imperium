using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGamePun : MonoBehaviour
{
    public static EndGamePun Instance { get; private set; } = null;
    private RaiseEventOptions raiseEventOptions;

    private const byte END_GAME = 40;
    private const byte END_GAME_DEFENDER = 41;

    [SerializeField]
    private TMP_Text loseText;
    [SerializeField]
    private TMP_Text counterText;
    [SerializeField]
    private GameObject endGameObject;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    }

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonData)
    {
        byte eventCode = photonData.Code;

        if (eventCode == END_GAME)
        {
            LoseEvent();
        }
        else if (eventCode == END_GAME_DEFENDER)
        {
            WinEvent();
        }
    }

    public void WinEvent()
    {
        GameManager.Instance.EnableOrDisablePlayerControl(false);
        endGameObject.SetActive(true);
        loseText.text = "You Win!";
        StartCoroutine(KickPlayer());
    }

    public void LoseEvent()
    {
        GameManager.Instance.EnableOrDisablePlayerControl(false);
        endGameObject.SetActive(true);
        loseText.text = "You Lose!";
        StartCoroutine(KickPlayer());
    }

    private IEnumerator KickPlayer()
    {
        int count = 4;
        for (int i = 0; i < 5; i++)
        {
            counterText.text = (count + " Seconds").ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        LevelLoader.Instance.LoadNextScene(2);
        if (Music.Instance.skipToMain)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public void SendData(byte byteCode)
    {
        PhotonNetwork.RaiseEvent(byteCode, null, raiseEventOptions, SendOptions.SendReliable);
    }
}
