using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    //Components 
    public GameObject rightShopUI;

    [SerializeField]
    private GameObject shopCardGroup;
    //[SerializeField]
    //private GameObject shopCard;
    private GameObject bigShopCard;
    [SerializeField]
    private TextMeshProUGUI herosGold;

    //Selected Card
    private GameObject selectedCard;

    //MinionPrefab
    public GameObject minionPrefab;


    public void Start()
    {
        GameManager.Instance.topHero = new Hero();
        GameManager.Instance.topHero.MyTurn = false;
        GameManager.Instance.topHero.Gold = 4;

        //Test
        GameManager.Instance.bottomHero = GameObject.Find("HeroPortrait").GetComponent<Hero>();
        GameManager.Instance.bottomHero.MyTurn = true;
        GameManager.Instance.bottomHero.CurrentHealth = 5;
        GameManager.Instance.bottomHero.TotalHealth = 5;
        GameManager.Instance.bottomHero.SetHealth();

        GameManager.Instance.bottomHero.Experience = 5;
        GameManager.Instance.bottomHero.RequredExp = 10;
        GameManager.Instance.bottomHero.SetExp();
        GameManager.Instance.bottomHero.GainExp(1);

        GameManager.Instance.bottomHero.CurrentMana = 3;
        GameManager.Instance.bottomHero.TotalMana = 6;
        GameManager.Instance.bottomHero.SetMana();
        GameManager.Instance.bottomHero.AdjustMana(4, false);

        GameManager.Instance.bottomHero.Gold = 5;
        GameManager.Instance.bottomHero.SetGold();

        GameManager.Instance.bottomHero.SetPlayerName("C067");

        GameManager.Instance.bottomHero.Level = 5;
        GameManager.Instance.bottomHero.SetLevel();
    }

    public void UpdateShopCard(GameObject selectedMinionObject)
    {
        //Delete the bigShopCard if there is an instance of it
        if (bigShopCard != null)
        {
            Destroy(bigShopCard.gameObject);
            bigShopCard = null;
        }        
        
        //Store the selected minion's object
        selectedCard = selectedMinionObject;
        //Get the MinionVisual from the GameObject
        MinionVisual selectedMinion = selectedMinionObject.GetComponent<MinionVisual>();

        //Spawn Card
        UIManager.Instance.currentMinion = selectedMinion.minionData;
        GameObject tmp = Instantiate(minionPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        tmp.transform.SetParent(shopCardGroup.transform);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        tmp.transform.localPosition = new Vector3(0f, 0f, 0f);

        //Set the BigShopCard object with the spawned card
        bigShopCard = tmp;

        //Update Hero's Current Gold in Shop
        int currentPlayer = GetPlayer();
        if (currentPlayer == 0)
        {
            herosGold.text = GameManager.Instance.bottomHero.Gold.ToString();            
        }
        else
        {
            herosGold.text = GameManager.Instance.topHero.Gold.ToString();
        }
    }

    public void BuyCard()
    {
        int costForCard = int.Parse(selectedCard.GetComponent<MinionVisual>().cost.text);
        int currentPlayer = GetPlayer();

        //Compare the cost of the Card
        if (currentPlayer == 0)
        {
            if (GameManager.Instance.bottomHero.Gold >= costForCard)
            {
                Debug.Log("Can Buy");
                //Get the Purchased Minion
                MinionVisual minion = selectedCard.GetComponent<MinionVisual>() as MinionVisual;

                //Subtract the Hero's current Gold
                GameManager.Instance.bottomHero.AdjustGold(costForCard, false);
                //Update the Gold UI
                herosGold.text = GameManager.Instance.bottomHero.Gold.ToString();

                //Move Minion in the Correct Discard Pile
                MoveMinionToDiscard(minion, currentPlayer);

                //Destroy the Object
                RemoveCard();

                //Close the Right Shop GUI
                rightShopUI.SetActive(false);
            }
            else
            {
                Debug.Log("Cannot buy");
            }
        }
        else
        {
            if (GameManager.Instance.topHero.Gold >= costForCard)
            {
                Debug.Log("Can Buy");
                //Get the Purchased Minion
                MinionVisual minion = selectedCard.GetComponent<MinionVisual>() as MinionVisual;

                //Subtract the Hero's current Gold
                GameManager.Instance.topHero.AdjustGold(costForCard, false);
                //Update the Gold UI
                herosGold.text = GameManager.Instance.topHero.Gold.ToString();

                //Move Minion in the Correct Discard Pile
                MoveMinionToDiscard(minion, currentPlayer);

                //Destroy the Object
                RemoveCard();

                //Close the Right Shop GUI
                rightShopUI.SetActive(false);
            }
            else
            {
                Debug.Log("Cannot buy");
            }
        }
    }

    public void ChangeShop()
    {
        int costToChangeCard = int.Parse(selectedCard.GetComponent<MinionVisual>().cost.text) / 2;
        int currentPlayer = GetPlayer();

        if (currentPlayer == 0)
        {
            if (GameManager.Instance.bottomHero.Gold >= costToChangeCard)
            {
                //Subtract the Current Gold and Update the UI                
                GameManager.Instance.bottomHero.AdjustGold(costToChangeCard, false);
                herosGold.text = GameManager.Instance.bottomHero.Gold.ToString();

                //Destroy the Object
                RemoveCard();

                //Close the Right Shop GUI
                rightShopUI.SetActive(false);
            }
            else
            {
                Debug.Log("Cannot Change Card");
            }
        }
        else
        {
            if (GameManager.Instance.topHero.Gold >= costToChangeCard)
            {
                //Subtract the Current Gold and Update the UI
                GameManager.Instance.topHero.AdjustGold(costToChangeCard, false);
                herosGold.text = GameManager.Instance.topHero.Gold.ToString();

                //Destroy the Object
                RemoveCard();

                //Close the Right Shop GUI
                rightShopUI.SetActive(false);
            }
            else
            {
                Debug.Log("Cannot Change Card");
            }
        }
    }

    private int GetPlayer()
    {
        int player = 0;

        if (GameManager.Instance.bottomHero.MyTurn)
        {
            player = 0; //bottom player
        }
        else
        {
            player = 1; //top player
        }

        return player;
    }

    public void CloseShop()
    {
        rightShopUI.SetActive(false);
    }

    private void RemoveCard()
    {
        //Destroy the Object
        Destroy(selectedCard.gameObject);
        selectedCard = null;
    }

    private void MoveMinionToDiscard(MinionVisual minion, int currentPlayer)
    {
        if (currentPlayer == 0)
        {
            GameObject tmp = SpawnMinion(minion);
            tmp.transform.SetParent(GameObject.Find("DiscardPile").transform, false);
            tmp.transform.localScale = new Vector3(1f, 1f, 1f);

            GameManager.Instance.alliedDiscardPile.Add(tmp);
        }
        else
        {
            GameObject tmp = SpawnMinion(minion);
            tmp.transform.SetParent(GameObject.Find("EnemyDiscardPile").transform, false);
            tmp.transform.localScale = new Vector3(1f, 1f, 1f);

            GameManager.Instance.enemyDiscardPile.Add(tmp);
        }
    }

    private GameObject SpawnMinion(MinionVisual minion)
    {
        UIManager.Instance.currentMinion = minion.minionData;
        GameObject tmp = Instantiate(minionPrefab) as GameObject;

        return tmp;
    }

    public void PopulateShop()
    {
        int num = Random.Range(1, 100);

        UIManager.Instance.currentMinion = Resources.Load("Minions/" + num) as MinionData;
        GameObject tmp = Instantiate(minionPrefab) as GameObject;
        tmp.transform.SetParent(GameObject.Find("DealtMinions").transform, false);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);

        tmp.AddComponent<ShowShopCard>();
        tmp.GetComponent<ShowShopCard>().shop = this.gameObject;
    }
}
