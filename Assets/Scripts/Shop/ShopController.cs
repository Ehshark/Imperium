using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    //Components 
    [SerializeField]
    private GameObject cardGroup;
    [SerializeField]
    private TextMeshProUGUI herosGold;
    [SerializeField]
    private Button changeShop;

    //Big Shop Card
    private GameObject bigShopCard;

    //Selected Card
    private GameObject selectedCard;

    public void Start()
    {
        //Update Hero's Current Gold in Shop
        Hero active = GameManager.Instance.ActiveHero();
        herosGold.text = active.Gold.ToString();
    }

    public void UpdateShopCard(GameObject selectedObject)
    {
        //Delete the bigShopCard if there is an instance of it
        if (bigShopCard != null)
        {
            Destroy(bigShopCard.gameObject);
            bigShopCard = null;
        }

        if (selectedObject != null)
        {
            //Store the selected minion's object
            selectedCard = selectedMinionObject;
            //Get the CardVisual from the GameObject
            CardVisual selectedMinion = selectedMinionObject.GetComponent<CardVisual>();

            if (selectedObject.GetComponent<MinionVisual>() != null)
            {
                //Get the MinionVisual from the GameObject
                MinionVisual selectedMinion = selectedObject.GetComponent<MinionVisual>();

                //Spawn Card
                GameObject tmp = GameManager.Instance.SpawnCard(cardGroup.transform, selectedMinion.Md);

                //Set the BigShopCard object with the spawned card
                bigShopCard = tmp;

                //Set ChangeShop to active 
                changeShop.interactable = true;
            }
            else if (selectedObject.GetComponent<EssentialVisual>() != null)
            {
                //Get the MinionVisual from the GameObject
                EssentialVisual selectedEssential = selectedObject.GetComponent<EssentialVisual>();                

                //Spawn Card
                GameObject tmp = GameManager.Instance.SpawnCard(cardGroup.transform, null, selectedEssential.Ed, null, true);

                //Set the BigShopCard object with the spawned card
                bigShopCard = tmp;

                //Set ChangeShop to inactive 
                changeShop.interactable = false;
            }
        }
    }

    public void BuyCard()
    {
        if (selectedCard != null)
        {
            int costForCard = int.Parse(selectedCard.GetComponent<CardVisual>().cost.text);
            int currentPlayer = GetPlayer();

            if (active.Gold >= costForCard)
            {
                if (GameManager.Instance.bottomHero.Gold >= costForCard)
                {
                    Debug.Log("Can Buy");
                    //Get the Purchased Minion
                    CardVisual minion = selectedCard.GetComponent<CardVisual>() as CardVisual;

                if (selectedCard.GetComponent<MinionVisual>() != null)
                {
                    //Get the Purchased Minion
                    MinionVisual minion = selectedCard.GetComponent<MinionVisual>() as MinionVisual;

                    //Spawn a new card from the correct deck
                    SpawnShopMinion(minion.Md.CardClass, false);

                    //Move Card to the Correct Discard Pile
                    MoveShopCardToDiscard(selectedCard);

                    //Destroy the Object
                    RemoveCard(true);
                }
                else if (selectedCard.GetComponent<EssentialVisual>() != null)
                {
                    //Get the Purchased Essential
                    EssentialVisual essential = selectedCard.GetComponent<EssentialVisual>() as EssentialVisual;

                    //Move Card to the Correct Discard Pile
                    MoveShopCardToDiscard(selectedCard);

                    //Destroy the Object
                    RemoveCard(false);
                }

                //Subtract the Hero's current Gold
                active.AdjustGold(costForCard, false);
                //Update the Gold UI
                herosGold.text = active.Gold.ToString();
            }
            else
            {
                if (GameManager.Instance.topHero.Gold >= costForCard)
                {
                    Debug.Log("Can Buy");
                    //Get the Purchased Minion
                    CardVisual minion = selectedCard.GetComponent<CardVisual>() as CardVisual;

                    //Subtract the Hero's current Gold
                    GameManager.Instance.topHero.AdjustGold(costForCard, false);
                    //Update the Gold UI
                    herosGold.text = GameManager.Instance.topHero.Gold.ToString();

                    //Move Minion in the Correct Discard Pile
                    MoveMinionToDiscard(minion, currentPlayer);

                    //Destroy the Object
                    RemoveCard();

                    //Spawn a new card from the correct deck
                    SpawnShopMinion(minion.Md.CardClass, false);
                }
                else
                {
                    Debug.Log("Cannot buy");
                }
            }
        }
    }

    public void ChangeShop()
    {
        if (selectedCard != null)
        {
            int costToChangeCard = int.Parse(selectedCard.GetComponent<CardVisual>().cost.text) / 2;
            int currentPlayer = GetPlayer();

            if (active.Gold >= costToChangeCard)
            {
                //Compare if there is Card's to change the shop
                if (UIManager.Instance.CanChangeShopCard(selectedCard.GetComponent<MinionVisual>().Md.CardClass))
                {
                    //Compare if there is Card's to change the shop
                    if (UIManager.Instance.CanChangeShopCard(selectedCard.GetComponent<CardVisual>().Md.CardClass))
                    {
                        //Spawn a new card from the correct deck
                        SpawnShopMinion(selectedCard.GetComponent<CardVisual>().Md.CardClass, true, selectedCard.GetComponent<CardVisual>().Md);

                    //Subtract the Current Gold and Update the UI                
                    active.AdjustGold(costToChangeCard, false);
                    herosGold.text = active.Gold.ToString();

                    //Destroy the Object
                    RemoveCard(true);
                }
                else
                {
                    Debug.Log("No Cards in pile");
                }
            }
            else
            {
                if (GameManager.Instance.topHero.Gold >= costToChangeCard)
                {
                    //Compare if there is Card's to change the shop
                    if (UIManager.Instance.CanChangeShopCard(selectedCard.GetComponent<CardVisual>().Md.CardClass))
                    {
                        //Spawn a new card from the correct deck
                        SpawnShopMinion(selectedCard.GetComponent<CardVisual>().Md.CardClass, true, selectedCard.GetComponent<CardVisual>().Md);

                        //Subtract the Current Gold and Update the UI
                        GameManager.Instance.topHero.AdjustGold(costToChangeCard, false);
                        herosGold.text = GameManager.Instance.topHero.Gold.ToString();

                        //Destroy the Object
                        RemoveCard();
                    }
                    else
                    {
                        Debug.Log("No Cards in pile");
                    }
                }
                else
                {
                    Debug.Log("Cannot Change Card");
                }
            }
        }
    }

    private void RemoveCard(bool destroyMinion)
    {
        if (destroyMinion)
        {
            //Destroy the Selected Card Object
            Destroy(selectedCard.gameObject);
            selectedCard = null;
        }
        else
        {
            selectedCard = null;
        }

        //Destroy the Big Card Object
        Destroy(bigShopCard.gameObject);
        bigShopCard = null;
    }

    private void MoveMinionToDiscard(CardVisual minion, int currentPlayer)
    {
        if (currentPlayer == 0)
        {
            GameObject tmp = SpawnMinion(minion);
            tmp.transform.SetParent(GameManager.Instance.alliedDiscardPile, false);
            tmp.transform.localScale = new Vector3(1f, 1f, 1f);

            GameManager.Instance.alliedDiscardPileList.Add(tmp);
        }
        else
        {
            GameObject tmp = SpawnMinion(minion);
            tmp.transform.SetParent(GameManager.Instance.enemyDiscardPile, false);
            tmp.transform.localScale = new Vector3(1f, 1f, 1f);

            GameManager.Instance.enemyDiscardPileList.Add(tmp);
        }
    }

    private GameObject SpawnMinion(CardVisual minion)
    {
        GameObject tmp = Instantiate(minionPrefab) as GameObject;
        tmp.SetActive(false);
        tmp.GetComponent<CardVisual>().Md = minion.Md;
        tmp.SetActive(true);

        return tmp;
    }

    private void SpawnShopMinion(string cardType, bool change, MinionData minionData = null)
    {
        GameObject tmp;

        if (change)
        {
            tmp = UIManager.Instance.GetNextShopCard(cardType, true, minionData);
        }
        else
        {
           tmp = UIManager.Instance.GetNextShopCard(cardType, false);
        }

        if (cardType.Equals("Warrior"))
        {
            tmp.transform.SetParent(GameManager.Instance.warriorShopPile, false);
            tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (cardType.Equals("Rogue"))
        {
            tmp.transform.SetParent(GameManager.Instance.rogueShopPile, false);
            tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            tmp.transform.SetParent(GameManager.Instance.mageShopPile, false);
            tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private int GetCostForCard(GameObject card)
    {
        if (card.GetComponent<MinionVisual>() != null)
        {
            return card.GetComponent<MinionVisual>().Md.GoldAndManaCost;
        }
        else if (card.GetComponent<EssentialVisual>() != null)
        {
            return card.GetComponent<EssentialVisual>().Ed.GoldCost;
        }
        else
        {
            return 0;
        }
    }

    private void MoveShopCardToDiscard(GameObject card)
    {
        int currentPlayer = GameManager.Instance.GetCurrentPlayer();

        if (currentPlayer == 0)
        {
            GameManager.Instance.MoveCard(selectedCard, GameManager.Instance.alliedDiscardPile, GameManager.Instance.alliedDiscardPileList);
        }
        else
        {
            GameManager.Instance.MoveCard(selectedCard, GameManager.Instance.enemyDiscardPile, GameManager.Instance.enemyDiscardPileList);
        }
    }
}
