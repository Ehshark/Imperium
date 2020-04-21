using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonConnector : MonoBehaviourPunCallbacks
{
    public Button connectButton;
    public Text connectButtonText;
    public Button playButton;
    public InputField userName;
    public string gameVersion = "0.0.1";
    // Start is called before the first frame update
    public void ConnectToPhoton()
    {
        if ((userName.text).Equals(""))
            return;

        PhotonNetwork.NickName = userName.text;
        GameManager.UserName = userName.text;
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        connectButton.interactable = false;
        connectButtonText.text = "Connecting...";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        playButton.interactable = true;
        connectButtonText.text = "Connected to Photon";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server for reason " + cause.ToString());
        connectButton.interactable = true;
        connectButtonText.text = "Disconnected from Photon. Click to connect.";
    }
}
