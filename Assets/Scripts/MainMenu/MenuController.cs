using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI userText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private RawImage avatarImage;

    //Match
    [SerializeField]
    private TMP_Text playButtonText;
    int seconds = 5;
    const string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
    int charAmount = 10;
    [SerializeField]
    private string roomName = "";
    private List<string> allRoomNames;
    List<RoomInfo> rooms;
    bool roomListUpdated = false;

    public void Awake()
    {
        if(GameManager.player != null)
        {
            userText.text = GameManager.player.PLAYER_USERNAME;
            levelText.text = GameManager.player.LEADERBOARD_LEVEL.ToString();

            LoadAvatar();
        }
    }

    private void LoadAvatar()
    {
        int height = 125;
        int width = 125;
        Texture2D target = new Texture2D(height, width);

        if (GameManager.player.PLAYER_AVATAR != null && GameManager.player.PLAYER_AVATAR.Length != 0)
        {
            target.LoadImage(GameManager.player.PLAYER_AVATAR);
            target.Apply();
            avatarImage.texture = target;
        }
        else
        {
            Texture2D photo = Resources.Load("VisualAssets/Images/Default") as Texture2D;
            avatarImage.texture = photo;
        }
    }

    public void PlayButton()
    {
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
        }
    }

    public void UpdatePlayButton()
    {
        playButtonText.text = "Found match. Starting in " + seconds + " seconds...";
        seconds--;
        if (seconds == 0)
        {
            LevelLoader.Instance.LoadNextScene(3);
            Music.Instance.PlayGameboardMusic();
        }
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

    public void LeaderboardClicked() 
    {
        LevelLoader.Instance.LoadNextScene(5);
    }

    public void OptionsClicked()
    {

    }

    public void CreditsClicked()
    {
        Application.OpenURL("https://imperium-site.herokuapp.com/Home/Credit");
    }

    public void QuitClicked()
    {
        Application.Quit();
    }
}
