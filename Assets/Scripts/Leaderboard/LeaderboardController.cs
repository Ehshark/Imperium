using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardController : MonoBehaviour
{
    private int playerRank = 0;
    [SerializeField]
    private GameObject playerRow;

    // Start is called before the first frame update
    void Start()
    {
        AppendTableRows();
        SetPlayerRow();
    }

    private void AppendTableRows()
    {
        IEnumerable<Leaderboard> leaderboardList = SQLManager.LeaderboardGetAll();

        int numOfRows = leaderboardList.Count();
        float rowHeight = 100;
        float height = rowHeight * (numOfRows + 1);

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);

        int cnt = 0;

        foreach (var leaderboard in leaderboardList)
        {
            //Get the player in the list
            leaderboard.player = SQLManager.GetPlayer(leaderboard.PLAYER_ID);

            if (GameManager.player != null)
            {
                if (GameManager.player.PLAYER_ID == leaderboard.PLAYER_ID)
                {
                    playerRank = cnt + 1;
                }
            }

            //Get, Instantiate and change the location of the Prefab 
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Row.prefab", typeof(Object));
            GameObject tmp = Instantiate(prefab) as GameObject;
            tmp.transform.SetParent(transform);
            tmp.transform.localPosition = new Vector3(0f, -rowHeight * cnt, 0f);

            //Set the values in the Prefab
            tmp.GetComponent<SetRow>().rank.text = (cnt + 1).ToString();
            tmp.GetComponent<SetRow>().username.text = leaderboard.player.PLAYER_USERNAME;
            tmp.GetComponent<SetRow>().level.text = leaderboard.LEADERBOARD_LEVEL.ToString();
            tmp.GetComponent<SetRow>().winrate.text = leaderboard.LEADERBOARD_WINRATE.ToString() + "%";
            tmp.GetComponent<SetRow>().wins.text = leaderboard.LEADERBOARD_WINS.ToString();
            tmp.GetComponent<SetRow>().losses.text = leaderboard.LEADERBOARD_LOSSES.ToString();
            tmp.GetComponent<SetRow>().avatar.texture = LoadAvatar(leaderboard.player.PLAYER_AVATAR);

            cnt++;
        }
    }

    private void SetPlayerRow()
    {
        if (GameManager.player != null)
        {
            Leaderboard leaderboard = SQLManager.LeaderboardGetOne(GameManager.player.PLAYER_ID);

            if (leaderboard.LEADERBOARD_ID != 0)
            {
                //Set the values in the Row
                playerRow.GetComponent<SetRow>().rank.text = playerRank.ToString();
                playerRow.GetComponent<SetRow>().username.text = GameManager.player.PLAYER_USERNAME;
                playerRow.GetComponent<SetRow>().level.text = leaderboard.LEADERBOARD_LEVEL.ToString();
                playerRow.GetComponent<SetRow>().winrate.text = leaderboard.LEADERBOARD_WINRATE.ToString() + "%";
                playerRow.GetComponent<SetRow>().wins.text = leaderboard.LEADERBOARD_WINS.ToString();
                playerRow.GetComponent<SetRow>().losses.text = leaderboard.LEADERBOARD_LOSSES.ToString();
                playerRow.GetComponent<SetRow>().avatar.texture = LoadAvatar(GameManager.player.PLAYER_AVATAR);
            }
        }
    }

    private Texture2D LoadAvatar(byte[] avatarByte)
    {
        int height = 125;
        int width = 125;
        Texture2D target = new Texture2D(height, width);

        if (avatarByte != null && avatarByte.Length != 0)
        {
            target.LoadImage(avatarByte);
            target.Apply();

            return target;
        }
        else
        {
            Texture2D photo = Resources.Load("VisualAssets/Images/Default") as Texture2D;

            return photo;
        }
    }

    public void BackClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
