using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LeaderboardClicked() 
    {
        SceneManager.LoadScene("Leaderboard");
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
