using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;

public class SQLManager : MonoBehaviour
{
    public static Player GetPlayer(string email)
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

                Debug.Log(rdr[2]);

                if (!DBNull.Value.Equals(rdr[2]))
                {
                    tmpPlayer.PLAYER_AVATAR = (byte[])rdr[2];
                }

                if (!DBNull.Value.Equals(rdr[3]))
                {
                    tmpPlayer.PLAYER_AVATARTYPE = rdr[3].ToString();
                }

                tmpPlayer.PLAYER_PASSWORD = rdr[4].ToString();
                //tmpPlayer.PLAYER_PASSWORDCOUNT = int.Parse(rdr[5].ToString());
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

            //Return the Player
            return tmpPlayer;
        }
    }

    public static int GetLeaderboardLevel(int id)
    {
        int level = 0;

        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        try
        {
            string sql = "SELECT LEADERBOARD_LEVEL " +
                     "FROM Leaderboard " +
                     "WHERE PLAYER_ID = " + id.ToString() + "";

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                level = int.Parse(rdr[0].ToString());
            }
        }
        catch(Exception)
        {
            level = 0;
        }

        return level;
    }
}
