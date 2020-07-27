using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class LoginPlayer : MonoBehaviour
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
                LevelLoader.Instance.LoadNextScene(0);
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
}
