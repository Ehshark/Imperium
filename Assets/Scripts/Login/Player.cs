using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Player
{
    public int PLAYER_ID { get; set; }

    public int LEADERBOARD_LEVEL { get; set; }

    public string PLAYER_USERNAME { get; set; }

    public byte[] PLAYER_AVATAR { get; set; }

    public string PLAYER_AVATARTYPE { get; set; }

    public string PLAYER_PASSWORD { get; set; }

    //public int PLAYER_PASSWORDCOUNT { get; set; }

    public string PLAYER_EMAIL { get; set; }

    public bool PLAYER_LOGGEDON { get; set; }

    public bool PLAYER_INGAME { get; set; }

    public DateTime PLAYER_DATEREGISTERED { get; set; }
}
