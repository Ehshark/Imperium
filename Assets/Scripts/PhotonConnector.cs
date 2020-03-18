using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonConnector : MonoBehaviourPunCallbacks
{
    public Button playButton;
    public InputField userName;
    public string gameVersion = "0.0.1";
    // Start is called before the first frame update
    public void ConnectToPhoton()
    {
        if ((userName.text).Equals(""))
            return;

        PhotonNetwork.NickName = userName.text;
        GameManager.userName = userName.text;
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        playButton.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server for reason " + cause.ToString());
    }
}
