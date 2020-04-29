using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Text;
using System;

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

    //Player Object
    protected static Player player = new Player();

    //Crypto
    private Crypto crypto = new Crypto();

    public void PlayerLogin()
    {
        //Get the email and password from the InputFields
        email = emailInput.text.ToString();
        password = passwordInput.text.ToString();

        //Using the email, get the player
        Player tmp = GetPlayer(email);

        //Compare if the Player was found
        if (tmp.PLAYER_ID != 0)
        {
            if (crypto.VerifyHashedPassword(tmp.PLAYER_PASSWORD, password))
            {
                errorText.text = "Player found: " + tmp.PLAYER_USERNAME;
                errorPanel.SetActive(true);
            }
            else
            {
                errorText.text = "Error: Password";
                errorPanel.SetActive(true);
            }
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

    private Player GetPlayer(string email)
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        try
        {
            da = new MySqlDataAdapter();

            //Create the SelectCommand
            string sql = "SELECT * " +
                         "FROM Players " +
                         "WHERE PLAYER_EMAIL='" + email + "'";

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            //Create a temp Player
            Player tmpPlayer = new Player();

            //Read and Store the Player if found
            while (rdr.Read())
            {
                tmpPlayer.PLAYER_ID = int.Parse(rdr[0].ToString());
                tmpPlayer.PLAYER_USERNAME = rdr[1].ToString();

                if (!DBNull.Value.Equals(rdr[2]))
                {
                    tmpPlayer.PLAYER_AVATAR = (byte[])rdr[2];
                }

                if (!DBNull.Value.Equals(rdr[3]))
                {
                    tmpPlayer.PLAYER_AVATARTYPE = rdr[3].ToString();
                }

                tmpPlayer.PLAYER_PASSWORD = rdr[4].ToString();
                tmpPlayer.PLAYER_PASSWORDCOUNT = int.Parse(rdr[5].ToString());
                tmpPlayer.PLAYER_EMAIL = rdr[6].ToString();
                tmpPlayer.PLAYER_LOGGEDON = (bool)rdr[7];
                tmpPlayer.PLAYER_INGAME = (bool)rdr[8];
                tmpPlayer.PLAYER_DATEREGISTERED = (DateTime)rdr[9];
            }

            //Close the Reader
            rdr.Close();

            //Return the Player
            return tmpPlayer;
        }
        catch
        {
            //Create a temp Player
            Player tmpPlayer = new Player();

            //Set Error Message
            errorText.text = "Their was a problem trying to retrieve your account. Please try again.";
            errorPanel.SetActive(true);

            //Return the Player
            return tmpPlayer;
        }
    }

    public void ForgotPassword()
    {
        Application.OpenURL("https://imperium-site.herokuapp.com/Account/ForgotPassword");
    }
}
