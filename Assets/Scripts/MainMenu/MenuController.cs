using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI userText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private RawImage avatarImage;

    public void Awake()
    {
        if(GameManager.player != null)
        {
            userText.text = GameManager.player.PLAYER_USERNAME;
            levelText.text = GameManager.player.LEADERBOARD_LEVEL.ToString();

            LoadAvatar();
        }
    }

    private void LoadAvatar()
    {
        int height = 125;
        int width = 125;
        Texture2D target = new Texture2D(height, width);

        if (GameManager.player.PLAYER_AVATAR != null && GameManager.player.PLAYER_AVATAR.Length != 0)
        {
            target.LoadImage(GameManager.player.PLAYER_AVATAR);
            target.Apply();
            avatarImage.texture = target;
        }
        else
        {
            Texture2D photo = Resources.Load("VisualAssets/Images/Default") as Texture2D;
            avatarImage.texture = photo;
        }
    }

    public void LeaderboardClicked() 
    {
        LevelLoader.Instance.LoadNextScene(4);
    }

    public void OptionsClicked()
    {

    }

    public void CreditsClicked()
    {
        Application.OpenURL("https://imperium-site.herokuapp.com/Home/Credit");
    }

    public void QuitClicked()
    {
        Application.Quit();
    }
}
