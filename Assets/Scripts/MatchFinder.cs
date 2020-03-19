using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchFinder : MonoBehaviourPunCallbacks
{
    public Text playButtonText;
    int seconds = 3;
    const byte START_MATCH_EVENT = 0;
    

    [SerializeField]
    private string roomName = "RandomRoom";

    public void CreateOrJoinRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        playButtonText.text = "Waiting for second player...";
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventRecieved;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            InvokeRepeating("UpdatePlayButton", 0, 1.0f);
            PhotonNetwork.RaiseEvent(START_MATCH_EVENT, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }

    public void UpdatePlayButton()
    {
        playButtonText.text = "Found match. Starting in " + seconds + " seconds...";
        seconds--;
        if (seconds == 0 && PhotonNetwork.IsMasterClient)
            SceneManager.LoadScene(1);
    }

    private void NetworkingClient_EventRecieved(EventData obj)
    {
        if (seconds > 0)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                InvokeRepeating("UpdatePlayButton", 0, 1.0f);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message, this);
    }

    void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventRecieved;
    }
}
