using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchFinder : MonoBehaviourPunCallbacks
{
    public TMP_Text playButtonText;
    int seconds = 5;
    const byte START_MATCH_EVENT = 0;
    const string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
    int charAmount = 10;
    [SerializeField]
    private string roomName = "";
    private List<string> allRoomNames;
    List<RoomInfo> rooms;
    bool roomListUpdated = false;

    private void Start()
    {
        allRoomNames = new List<string>();
    }

    public void CreateOrJoinRoom()
    {
        playButtonText.transform.parent.GetComponent<Button>().interactable = false;
        if (!PhotonNetwork.IsConnected)
            return;

        //Join the lobby to trigger the OnRoomListUpdate callback
        PhotonNetwork.JoinLobby();

        //Call a coroutine to find the match
        StartCoroutine(FindMatch());
    }

    IEnumerator FindMatch()
    {
        //Wait until OnRoomListUpdate is successfully called
        yield return new WaitUntil(() => roomListUpdated = true);
        yield return new WaitForSeconds(1);

        //Loop through existing rooms to find an open room
        if (rooms != null)
        {
            foreach (RoomInfo r in rooms)
            {
                //Open room found
                if (r.PlayerCount != r.MaxPlayers)
                {
                    //Join Room
                    PhotonNetwork.JoinRoom(r.Name);
                    //Exit the coroutine
                    yield break;
                }

                //Room exists so store it's name
                allRoomNames.Add(r.Name);
            }
        }

        //We didn't find any open games, so let's create one
        //Generate a random string to use as room name
        roomName = "";
        for (int i = 0; i < charAmount; i++)
        {
            roomName += characters[Random.Range(0, characters.Length)];
        }

        //Check if roomName is unique
        if (rooms != null)
        {
            if (allRoomNames != null)
            {
                foreach (string n in allRoomNames)
                {
                    //Room name is not unique
                    if (n.Equals(roomName))
                    {
                        //generate a new room name
                        roomName = "";
                        for (int i = 0; i < charAmount; i++)
                        {
                            roomName += characters[Random.Range(0, characters.Length)];
                        }
                    }
                }
            }
        }

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2
        };

        //Create the room
        PhotonNetwork.CreateRoom(roomName, options);

        //Clear the list of room names
        if (allRoomNames != null)
            allRoomNames.Clear();
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
        playButtonText.transform.parent.GetComponent<Button>().interactable = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //Update the rooms field with the new roomList
        rooms = roomList;
    }

    public override void OnJoinedLobby()
    {
        //To accomodate for network delays, use a boolean and a WaitUntil to reduce chances of error
        roomListUpdated = true;
    }

    void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventRecieved;
    }
}
