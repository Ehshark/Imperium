using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine.Windows;

public class DatabaseManager : MonoBehaviour
{
    public static MySqlConnection connection;
    private string server, database, username, password;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (connection == null)
        {
            Initialize();
            bool result = OpenConnection();

            Debug.Log("Is Connected: " + result);
        }
        else
        {
            Debug.Log("Connection is established already");
        }
    }

    public void OnApplicationQuit()
    {
        bool result = CloseConnection();
        connection = null;

        Destroy(this.gameObject);

        Debug.Log("Connection is Closed? : " + result);
    }

    private void Initialize()
    {
        server = "mymysql.senecacollege.ca";
        database = "db_cdodge1";
        username = "db_cdodge1";
        password = "}c89qfXn)Y";

        string connectionString = "SERVER=" + server + ";" +
                                  "DATABASE=" + database + ";" +
                                  "UID=" + username + ";" +
                                  "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
    }

    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }

    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }
}
