using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour
{
    //Components
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private GameObject coinToss;
    [SerializeField]
    private TextMeshProUGUI reactionText;
    private Animator animator;

    [SerializeField]
    private GameObject heroUI;
    [SerializeField]
    private GameObject warrior;
    [SerializeField]
    private GameObject rogue;
    [SerializeField]
    private GameObject mage;

    public Sprite warriorImage;
    public Sprite rogueImage;
    public Sprite mageImage;

    [SerializeField]
    private GameObject bottomHero;
    [SerializeField]
    private GameObject topHero;
    private int turn = 0;
    private int cnt = 0;

    //Variables 
    private enum Coin { IDLE, TAILS, HEADS }
    private Coin coinValue;

    // Start is called before the first frame update
    void Start()
    {
        //Get the animator component in Coin
        animator = coin.GetComponent<Animator>();

        //Generate a random number and see which Player goes first
        int value = Random.Range(0, 1000);        

        if (value % 2 == 0)
        {
            reactionText.text = "Player 1 Chooses...";
            turn = 0;
        }
        else
        {
            reactionText.text = "Player 2 Chooses...";
            turn = 1;
        }

        coinValue = Coin.IDLE;

        //Setup each Player
        GameManager.Instance.bottomHero = bottomHero.GetComponent<Hero>();
        GameManager.Instance.topHero = topHero.GetComponent<Hero>();
        GameManager.Instance.bottomHero.SetPlayerName("Player 1");
        GameManager.Instance.topHero.SetPlayerName("Player 2");
    }

    public void FlipCoin(int value)
    {
        StartCoroutine(FlipCoinAnimation(value));
    }

    IEnumerator FlipCoinAnimation(int value)
    {
        animator.SetBool("flip", true);        
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("flip", false);

        if (turn == 0)
        {
            if ((int)coinValue == value)
            {
                reactionText.text = "You Win!";
                GameManager.Instance.bottomHero.MyTurn = true;
                GameManager.Instance.topHero.MyTurn = false;
            }
            else
            {
                reactionText.text = "You Lose!";
                GameManager.Instance.topHero.MyTurn = true;
                GameManager.Instance.bottomHero.MyTurn = false;
            }
        }
        else
        {
            if ((int)coinValue == value)
            {
                reactionText.text = "You Win!";
                GameManager.Instance.topHero.MyTurn = true;
                GameManager.Instance.bottomHero.MyTurn = false;
            }
            else
            {
                reactionText.text = "You Lose!";
                GameManager.Instance.bottomHero.MyTurn = true;
                GameManager.Instance.topHero.MyTurn = false;
            }
        }

        yield return new WaitForSeconds(2f);

        coinToss.SetActive(false);
        reactionText.text = "";
        ShowHeroUI();
    }

    public int GetCoinValue()
    {
        int value = Random.Range(0, 1000);
        int result;

        if (value % 2 == 0)
        {
            result = 0;
            coinValue = Coin.TAILS;
        }
        else
        {
            result = 1;
            coinValue = Coin.HEADS;
        }

        return result;
    }

    private void ShowHeroUI()
    {
        if (!heroUI.activeSelf)
        {
            heroUI.SetActive(true);
        }

        if (GameManager.Instance.bottomHero.MyTurn)
        {
            reactionText.text = "Player 1 Chooses....";
        }
        else
        {
            reactionText.text = "Player 2 Chooses....";
        }
    }

    public void SetHero(int hero)
    {
        //Warrior
        if (hero == 0)
        {
            if (GameManager.Instance.bottomHero.MyTurn)
            { 
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
            }
            else
            {
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 5, 'W', warriorImage);
            }

            warrior.SetActive(false);
        }
        //Rogue
        else if (hero == 1)
        {
            if (GameManager.Instance.bottomHero.MyTurn)
            {
                GameManager.Instance.bottomHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            }
            else
            {
                GameManager.Instance.topHero.SetHero(4, 4, 2, 6, 5, 'R', rogueImage);
            }

            rogue.SetActive(false);
        }
        //Mage 
        else
        {
            if (GameManager.Instance.bottomHero.MyTurn)
            {
                GameManager.Instance.bottomHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
            }
            else
            {
                GameManager.Instance.topHero.SetHero(4, 4, 1, 6, 6, 'M', mageImage);
            }

            mage.SetActive(false);
        }

        GameManager.Instance.SwitchTurn();
        cnt++;

        if (cnt == 2)
        {
            heroUI.SetActive(false);
            reactionText.text = "";
        }
        else
        {
            ShowHeroUI();
        }        
    }
}
