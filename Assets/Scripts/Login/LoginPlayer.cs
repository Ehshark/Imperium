using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Text;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LoginPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField emailInput = null;
    [SerializeField]
    private InputField passwordInput = null;
    [SerializeField]
    private Text errorText = null;
    [SerializeField]
    private GameObject errorPanel = null;

    //Variables
    private string email;
    private string password;

    //Crypto
    private Crypto crypto = new Crypto();

    public string gameVersion = "0.0.1";

    public void PlayerLogin()
    {
        //Get the email and password from the InputFields
        email = emailInput.text.ToString();
        password = passwordInput.text.ToString();

        //Using the email, get the player
        Player tmp = SQLManager.GetPlayer(email);

        //Compare if the Player was found
        if (tmp.PLAYER_ID != 0)
        {
            if (crypto.VerifyHashedPassword(tmp.PLAYER_PASSWORD, password))
            {
                tmp.LEADERBOARD_LEVEL = SQLManager.GetLeaderboardLevel(tmp.PLAYER_ID);
                GameManager.player = tmp;

                PhotonNetwork.NickName = GameManager.player.PLAYER_USERNAME;
                Debug.Log("Connecting to Photon...");
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                errorText.text = "The password entered was not correct. Please re-enter your password and try again.";
                errorPanel.SetActive(true);
            }
        }
        else if (tmp.PLAYER_USERNAME == null)
        {
            errorText.text = "Their was a problem trying to retrieve your account. Please try again.";
            errorPanel.SetActive(true);
        }
        else
        {
            if (errorText.text == "")
            {
                errorText.text = "The following email doesn't seem to have an account with us. Please try again.";
                errorPanel.SetActive(true);
            }
        }
    }

    public void ForgotPassword()
    {
        Application.OpenURL("https://imperium-site.herokuapp.com/Account/ForgotPassword");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        LevelLoader.Instance.LoadNextScene(2);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server for reason " + cause.ToString());
        errorText.text = "Couldn't Connect to the Photon Server. Please try Again.";
    }
}
