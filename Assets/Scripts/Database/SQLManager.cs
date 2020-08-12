using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Linq;

public class SQLManager : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////
    ///                          PLAYER                                 ///
    ///////////////////////////////////////////////////////////////////////

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

    public static Player GetPlayerByUsername(string username)
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
                         "WHERE PLAYER_USERNAME='" + username + "'";

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

    public static bool UpdatePlayerInBattle(string username, bool inBattle)
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        try
        {
            string sql;

            if (inBattle)
            {
                sql = "UPDATE Players " +
                      "SET PLAYER_INGAME = 1 " +
                      "WHERE PLAYER_USERNAME = '" + username + "'";
            }
            else
            {
                sql = "UPDATE Players " +
                      "SET PLAYER_INGAME = 0 " +
                      "WHERE PLAYER_USERNAME = '" + username + "'";
            }

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

            }

            //Close the Reader
            rdr.Close();

            return true;
        }
        catch (MySqlException ex)
        {
            return false;
        }
    }

    public static bool UpdatePlayerLoggedIn(string username, bool inBattle)
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        try
        {
            string sql;

            if (inBattle)
            {
                sql = "UPDATE Players " +
                      "SET PLAYER_LOGGEDON = 1 " +
                      "WHERE PLAYER_USERNAME = '" + username + "'";
            }
            else
            {
                sql = "UPDATE Players " +
                      "SET PLAYER_LOGGEDON = 0 " +
                      "WHERE PLAYER_USERNAME = '" + username + "'";
            }

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

            }

            //Close the Reader
            rdr.Close();

            return true;
        }
        catch (MySqlException ex)
        {
            return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////
    ///                         LEADERBOARD                             ///
    ///////////////////////////////////////////////////////////////////////

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
                                      .ThenByDescending(l => l.LEADERBOARD_WINS)
                                      .ThenByDescending(l => l.LEADERBOARD_WINRATE);

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

    public static bool UpdatePlayerWins(string playerUsername)
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        Player player = GetPlayerByUsername(playerUsername);
        Leaderboard leaderboard = LeaderboardGetOne(player.PLAYER_ID);

        try
        {
            int wins = leaderboard.LEADERBOARD_WINS + 1;
            int gamesPlayed = leaderboard.LEADERBOARD_GAMESPLAYED + 1;
            double percentage = ((double)wins / (double)gamesPlayed) * 100;
            int winrate = (int)percentage;

            string sql = "UPDATE Leaderboard " +
                         "SET LEADERBOARD_WINS = " + wins + ", " +
                             "LEADERBOARD_GAMESPLAYED = " + gamesPlayed + ", " +
                             "LEADERBOARD_WINRATE = " + winrate + " " +
                         "WHERE PLAYER_ID = " + leaderboard.PLAYER_ID;

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                
            }

            //Close the Reader
            rdr.Close();

            return true;
        }
        catch (Exception)
        {
            return false;   
        }
    }

    public static bool UpdatePlayerLose(string playerUsername)
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        Player player = GetPlayerByUsername(playerUsername);
        Leaderboard leaderboard = LeaderboardGetOne(player.PLAYER_ID);

        try
        {
            int losses = leaderboard.LEADERBOARD_LOSSES + 1;
            int gamesPlayed = leaderboard.LEADERBOARD_GAMESPLAYED + 1;
            double percentage = ((double)leaderboard.LEADERBOARD_WINS / (double)gamesPlayed) * 100;
            int winrate = (int)percentage;

            string sql = "UPDATE Leaderboard " +
                         "SET LEADERBOARD_LOSSES = " + losses + ", " +
                             "LEADERBOARD_GAMESPLAYED = " + gamesPlayed + ", " +
                             "LEADERBOARD_WINRATE = " + winrate + " " +
                         "WHERE PLAYER_ID = " + leaderboard.PLAYER_ID;

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

            }

            //Close the Reader
            rdr.Close();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////
    ///                          BATTLE                                 ///
    ///////////////////////////////////////////////////////////////////////

    public static bool CreateBattle(string player1Username, string player2Username)
    {
        Player player1 = GetPlayerByUsername(player1Username);
        Player player2 = GetPlayerByUsername(player2Username);

        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        try
        {
            string sql = "INSERT INTO Battle (BATTLE_ID, PLAYER_1, PLAYER_2, STATUS) " +
                "VALUES (NULL, '" + player1.PLAYER_ID.ToString() + "', '" + player2.PLAYER_ID.ToString() + "', 0)";

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                
            }

            //Close the Reader
            rdr.Close();

            return true;
        }
        catch (MySqlException ex)
        {
            return false;
        }
    }

    public static Battle GetCurrentBattle(string player1Username, string player2Username)
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        Player player1 = GetPlayerByUsername(player1Username);
        Player player2 = GetPlayerByUsername(player2Username);

        Battle battle = new Battle();

        try
        {
            string sql = "SELECT * " +
                         "FROM Battle " +
                         "WHERE PLAYER_1 IN (" + player1.PLAYER_ID.ToString() + ", " + player2.PLAYER_ID.ToString() + ") " +
                            "OR PLAYER_2 IN (" + player1.PLAYER_ID.ToString() + ", " + player2.PLAYER_ID.ToString() + ") " +
                         "ORDER BY BATTLE_ID DESC LIMIT 1";

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                battle.BATTLE_ID = int.Parse(rdr[0].ToString());
                battle.PLAYER_1 = int.Parse(rdr[1].ToString());
                battle.PLAYER_2 = int.Parse(rdr[2].ToString());
                battle.STATUS = int.Parse(rdr[3].ToString());
            }

            //Close the Reader
            rdr.Close();
        }
        catch (Exception)
        {
            battle = new Battle();
        }

        return battle;
    }

    public static bool UpdateBattleStatus(int battleId)
    {
        MySqlDataAdapter da = new MySqlDataAdapter();
        MySqlCommand cmd;
        MySqlDataReader rdr;

        try
        {
            string sql = "UPDATE Battle " +
                         "SET STATUS = 1 " +
                         "WHERE BATTLE_ID = " + battleId.ToString();

            cmd = new MySqlCommand(sql, DatabaseManager.connection);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                
            }

            //Close the Reader
            rdr.Close();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
