using System.Collections;
using System.Collections.Generic;

public class Leaderboard
{
    public int LEADERBOARD_ID { get; set; }

    public int PLAYER_ID { get; set; }

    public Player player { get; set; }

    public int LEADERBOARD_LEVEL { get; set; }

    public int LEADERBOARD_WINS { get; set; }

    public int LEADERBOARD_LOSSES { get; set; }

    public int LEADERBOARD_GAMESPLAYED { get; set; }

    public int LEADERBOARD_WINRATE { get; set; }
}
