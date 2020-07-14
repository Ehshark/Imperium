using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public TMP_Text pOneNameText;
    public TMP_Text pTwoNameText;
    public TMP_Text instructionsText;
    const byte UPDATE_DIDWINTOSS_EVENT = 1;
    bool didWinToss;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pOneNameText.text = "Host: " + PhotonNetwork.NickName;
            pTwoNameText.text = "Client: " + PhotonNetwork.PlayerListOthers[0].NickName;
        }

        else
        {
            pOneNameText.text = "Client: " + PhotonNetwork.NickName;
            pTwoNameText.text = "Host: " + PhotonNetwork.PlayerListOthers[0].NickName;
        }

        if (PhotonNetwork.IsMasterClient)
            GetTossResult();
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == UPDATE_DIDWINTOSS_EVENT)
            {
                didWinToss = (bool)photonEvent.CustomData;
            }
        }

        instructionsText.text = "Coin toss result is: " + didWinToss;
    }

    void GetTossResult()
    {
        int value = Random.Range(0, 1000);

        if (value % 2 == 0)
            didWinToss = true;

        else
            didWinToss = false;

        PhotonNetwork.RaiseEvent(UPDATE_DIDWINTOSS_EVENT, didWinToss, RaiseEventOptions.Default, SendOptions.SendReliable);
    }
}
