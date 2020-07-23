using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System;

public class MigrationManager : MonoBehaviourPunCallbacks
{
    public static MigrationManager Instance { get; private set; } = null;    

    public Text bottom;
    public Text top;
    public Text buttonText;

    public Manager Host = new Manager();
    public Manager Client = new Manager();
    public int count = 0;

    private const byte PLAYER_REJOIN_EVENT = 0;
    private const byte HOST_LEFT_EVENT = 1;
    private const byte CLIENT_LEFT_EVENT = 2;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SetHostClientUI();
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectPlayerCo());
    }

    public IEnumerator DisconnectPlayerCo()
    {
        //MinionData minion1 = Resources.Load("Minions/" + 1) as MinionData;
        //MinionData minion2 = Resources.Load("Minions/" + 2) as MinionData;
        //MinionData minion3 = Resources.Load("Minions/" + 3) as MinionData;
        //MinionDataPhoton minionDataPhoton1 = new MinionDataPhoton(minion1);
        //MinionDataPhoton minionDataPhoton2 = new MinionDataPhoton(minion2);
        //MinionDataPhoton minionDataPhoton3 = new MinionDataPhoton(minion3);

        //MinionDataPhoton[] minions = new MinionDataPhoton[] { minionDataPhoton1, minionDataPhoton2, minionDataPhoton3 };

        byte[] codeArray;
        if (PhotonNetwork.IsMasterClient)
        {
            codeArray = DataHandler.Instance.ObjectToByteArray(Host);
        }
        else
        {
            codeArray = DataHandler.Instance.ObjectToByteArray(Client);
        }
        //byte[] minionArray = DataHandler.Instance.ObjectToByteArray(minions);

        //object[] data = new object[] { codeArray, minionArray };
        object[] data = new object[] { codeArray };

        if (PhotonNetwork.IsMasterClient)
        {
            DataHandler.Instance.SendData(HOST_LEFT_EVENT, data);
        }
        else
        {
            DataHandler.Instance.SendData(CLIENT_LEFT_EVENT, data);
        }

        yield return new WaitForSeconds(2f);

        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        PhotonNetwork.SetMasterClient(newMasterClient);
        SetHostClientUI();
        Debug.Log("Master Switched");
    }

    public void SetHostClientUI()
    {
        string topPlayer = "";
        if (PhotonNetwork.PlayerListOthers.Length > 0)
        {
            topPlayer = PhotonNetwork.PlayerListOthers[0].NickName;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            bottom.text = PhotonNetwork.NickName + " : Host";
            top.text = topPlayer;
        }
        else
        {
            bottom.text = PhotonNetwork.NickName;
            top.text = topPlayer + " : Host";
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        SetHostClientUI();

        byte[] dataArray = DataHandler.Instance.ObjectToByteArray(Client);
        object[] data = new object[] { dataArray };

        DataHandler.Instance.SendData(PLAYER_REJOIN_EVENT, data);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        SetHostClientUI();
    }

    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        Debug.Log("Left Room");
    }

    public void ButtonClick(Text text)
    {
        text.text = (int.Parse(text.text) + 1).ToString();
        if (PhotonNetwork.IsMasterClient)
        {
            Host.val++;
        }
        else
        {
            Client.val++;
        }
    }
}

[Serializable]
public class Manager
{
    public int val { get; set; }
}