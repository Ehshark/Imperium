﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Linq;

public class SQLManager : MonoBehaviour
{
    public static Player GetPlayer(int id)
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
                         "WHERE PLAYER_ID = " + id.ToString() + "";

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

            //Close the Reader
            rdr.Close();
        }
        catch(Exception)
        {
            level = 0;
        }

        return level;
    }

    public static IEnumerable<Leaderboard> LeaderboardGetAll()
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        IEnumerable<Leaderboard> leaderboardList = new List<Leaderboard>();
        List<Leaderboard> leaderboards = new List<Leaderboard>();

        try
        {
            string sql = "SELECT * " +
                         "FROM Leaderboard ";

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Leaderboard leaderboard = new Leaderboard();

                leaderboard.LEADERBOARD_ID = int.Parse(rdr[0].ToString());
                leaderboard.PLAYER_ID = int.Parse(rdr[1].ToString());
                leaderboard.LEADERBOARD_LEVEL = int.Parse(rdr[2].ToString());
                leaderboard.LEADERBOARD_WINS = int.Parse(rdr[3].ToString());
                leaderboard.LEADERBOARD_LOSSES = int.Parse(rdr[4].ToString());
                leaderboard.LEADERBOARD_GAMESPLAYED = int.Parse(rdr[5].ToString());
                leaderboard.LEADERBOARD_WINRATE = int.Parse(rdr[6].ToString());

                leaderboards.Add(leaderboard);
            }

            //Close the Reader
            rdr.Close();
        }
        catch (Exception)
        {
            leaderboards = new List<Leaderboard>();
        }

        leaderboardList = leaderboards.OrderByDescending(l => l.LEADERBOARD_LEVEL)
                                      .ThenByDescending(l => l.LEADERBOARD_WINRATE)
                                      .ThenByDescending(l => l.LEADERBOARD_WINS);

        return leaderboardList;
    }

    public static Leaderboard LeaderboardGetOne(int id)
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        Leaderboard leaderboard = new Leaderboard();

        try
        {
            string sql = "SELECT * " +
                         "FROM Leaderboard " +
                         "WHERE PLAYER_ID = " + id.ToString() + "";

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                leaderboard.LEADERBOARD_ID = int.Parse(rdr[0].ToString());
                leaderboard.PLAYER_ID = int.Parse(rdr[1].ToString());
                leaderboard.LEADERBOARD_LEVEL = int.Parse(rdr[2].ToString());
                leaderboard.LEADERBOARD_WINS = int.Parse(rdr[3].ToString());
                leaderboard.LEADERBOARD_LOSSES = int.Parse(rdr[4].ToString());
                leaderboard.LEADERBOARD_GAMESPLAYED = int.Parse(rdr[5].ToString());
                leaderboard.LEADERBOARD_WINRATE = int.Parse(rdr[6].ToString());
            }

            //Close the Reader
            rdr.Close();
        }
        catch (Exception)
        {
            leaderboard = new Leaderboard();
        }

        return leaderboard;
    }
}
