using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class ShopController : MonoBehaviour
{
    //Components 
    public GameObject cardGroup;
    [SerializeField]
    private TextMeshProUGUI herosGold;
    [SerializeField]
    private Button changeShop;
    [SerializeField]
    private ParticleSystem exitGlow;

    //Big Shop Card
    private GameObject bigShopCard;

    //Selected Card
    private GameObject selectedCard;

    //Current Effect Card
    private GameObject card;
    public GameObject Card { get => card; set => card = value; }
    public GameObject BigShopCard { get => bigShopCard; set => bigShopCard = value; }

    public Transform warriorDeck;
    public Transform rogueDeck;
    public Transform mageDeck;
    public Transform goldPileIcon;

    //Multiplayer
    const byte BUY_CARD_SYNC_EVENT = 21;
    const byte CHANGE_SHOP_SYNC_EVENT = 22;

    private void OnEnable()
    {
        //Update Hero's Current Gold in Shop
        Hero active = GameManager.Instance.bottomHero;
        herosGold.text = active.Gold.ToString();
    }

    public void CloseShop()
    {
        exitGlow.gameObject.SetActive(false);
        gameObject.SetActive(false);
        goldPileIcon.GetComponent<Image>().color = Color.white;

        if (bigShopCard != null)
        {
            Destroy(bigShopCard.gameObject);
            bigShopCard = null;
        }

        if (StartGameController.Instance.tutorial)
        {
            StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
        }
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
            //Delete selectedCard if there's an instance of it 
            if (selectedCard != null)
            {
                selectedCard = null;
            }

            //Store the selected minion's object
            selectedCard = selectedObject;
            //Get the CardVisual from the GameObject
            CardVisual selectedVisual = selectedObject.GetComponent<CardVisual>();

            if (selectedVisual.Md != null)
            {
                //Spawn Card
                GameObject tmp = GameManager.Instance.SpawnCard(cardGroup.transform, selectedVisual.Md, true);
                tmp.AddComponent<EnlargedCardBehaviour>();
                //Set the BigShopCard object with the spawned card
                bigShopCard = tmp;

                if (!StartGameController.Instance.tutorial)
                {
                    //Set ChangeShop to active 
                    changeShop.interactable = true;
                }
            }
            else if (selectedVisual.Ed != null)
            {
                //Spawn Card
                GameObject tmp = GameManager.Instance.SpawnCard(cardGroup.transform, selectedVisual.Ed, true);

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
            Hero active = GameManager.Instance.ActiveHero(true);

            if (active.Gold >= costForCard)
            {
                goldPileIcon.GetComponent<Image>().color = Color.white;
                GameManager.Instance.buyButton.interactable = false;
                DelayCommand dc = new DelayCommand(goldPileIcon, 1f);
                dc.AddToQueue();
                //Get the Purchased Minion
                CardVisual minion = selectedCard.GetComponent<CardVisual>();

                object[] data = new object[] { minion.Md.CardClass, minion.Md.MinionID };
                PhotonNetwork.RaiseEvent(BUY_CARD_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                        SendOptions.SendReliable);

                if (selectedCard.GetComponent<CardVisual>().Md != null)
                {
                    //Spawn a new card from the correct deck
                    SpawnShopMinion(minion.Md.CardClass, false);

                    if (GameManager.Instance.HasExpressBuy)
                    {
                        MoveShopCardToHand(selectedCard);
                    }
                    else
                    {
                        //Move Card to the Correct Discard Pile
                        MoveShopCardToDiscard(selectedCard);
                    }

                    //Destroy minion card the big card objects
                    RemoveCard(true);
                }
                else if (selectedCard.GetComponent<CardVisual>().Ed != null)
                {
                    if (GameManager.Instance.HasExpressBuy)
                    {
                        MoveShopCardToHand(selectedCard);
                    }
                    else
                    {
                        //Move Card to the Correct Discard Pile
                        MoveShopCardToDiscard(selectedCard);
                    }

                    //Destroy only the big card object
                    RemoveCard(false);
                }

                //Subtract the Hero's current Gold
                active.AdjustGold(costForCard, false);
                //Update the Gold UI
                herosGold.text = active.Gold.ToString();

                //Buy First Card Condition
                foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
                {
                    if (!GameManager.Instance.BuyFirstCard)
                    {
                        ConditionListener cl = t.gameObject.GetComponent<ConditionListener>();
                        if (cl != null && cl.ConditionEvent == EVENT_TYPE.BUY_FIRST_CARD)
                        {
                            //EventManager.Instance.PostNotification(EVENT_TYPE.BUY_FIRST_CARD);
                            EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.BUY_FIRST_CARD);
                            GameManager.Instance.BuyFirstCard = true;
                            exitGlow.gameObject.SetActive(true);
                            break;
                        }
                    }
                }

                //Tutorial 
                if (StartGameController.Instance.tutorial)
                {
                    StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
                }
            }
            else
            {
                goldPileIcon.GetComponent<Image>().color = Color.red;
            }
        }
    }

    public void ChangeShop()
    {
        if (selectedCard != null)
        {
            if (GameManager.Instance.IsEffect)
            {
                ChangeShopEffect();
                return;
            }

            int costToChangeCard = int.Parse(selectedCard.GetComponent<CardVisual>().cost.text) / 2;
            Hero active = GameManager.Instance.ActiveHero(true);

            if (active.Gold >= costToChangeCard)
            {
                //Compare if there is Card's to change the shop
                if (UIManager.Instance.CanChangeShopCard(selectedCard.GetComponent<CardVisual>().Md.CardClass))
                {
                    goldPileIcon.GetComponent<Image>().color = Color.white;
                    object[] data = new object[] { selectedCard.GetComponent<CardVisual>().Md.CardClass, selectedCard.GetComponent<CardVisual>().Md.MinionID };
                    PhotonNetwork.RaiseEvent(CHANGE_SHOP_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                            SendOptions.SendReliable);

                    //Delay on Gold Coin
                    DelayCommand dc = new DelayCommand(goldPileIcon, 1f);
                    dc.AddToQueue();

                    //Delay on Pile
                    DelayOnPile();

                    //Spawn a new card from the correct deck
                    SpawnShopMinion(selectedCard.GetComponent<CardVisual>().Md.CardClass, true, selectedCard.GetComponent<CardVisual>().Md);

                    //Subtract the Current Gold and Update the UI                
                    active.AdjustGold(costToChangeCard, false);
                    herosGold.text = active.Gold.ToString();

                    //Destroy the Object
                    RemoveCard(true);

                    //First Change Shop Card Condition
                    foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
                    {
                        if (!GameManager.Instance.FirstChangeShop)
                        {
                            ConditionListener cl = t.gameObject.GetComponent<ConditionListener>();
                            if (cl != null && cl.ConditionEvent == EVENT_TYPE.FIRST_CHANGE_SHOP)
                            {
                                //EventManager.Instance.PostNotification(EVENT_TYPE.FIRST_CHANGE_SHOP);
                                EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.FIRST_CHANGE_SHOP);
                                GameManager.Instance.FirstChangeShop = true;
                                exitGlow.gameObject.SetActive(true);
                                break;
                            }
                        }
                    }

                    //Tutorial
                    if (StartGameController.Instance.tutorial)
                    {
                        StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
                    }
                }
                else
                {
                    Debug.Log("No Cards in pile");
                }
            }
            else
            {
                goldPileIcon.GetComponent<Image>().color = Color.red;
            }
        }
    }

    private void ChangeShopEffect()
    {
        //Spawn a new card from the correct deck
        SpawnShopMinion(selectedCard.GetComponent<CardVisual>().Md.CardClass, true, selectedCard.GetComponent<CardVisual>().Md);

        //Destroy the Object
        RemoveCard(true);

        card.GetComponent<ChangeShopListener>().EnableShop();
    }

    public void RemoveCard(bool destroyMinion)
    {
        if (destroyMinion)
        {
            selectedCard.SetActive(false);
        }
        else
        {
            selectedCard = null;
        }

        //Destroy the Big Card Object
        Destroy(bigShopCard.gameObject);
        bigShopCard = null;
    }

    public void SpawnShopMinion(string cardType, bool change, MinionData minionData = null)
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

    private int GetCostForCard(GameObject card) //being used by buttons in shop
    {
        if (card.GetComponent<CardVisual>().Md != null)
        {
            return card.GetComponent<CardVisual>().Md.GoldAndManaCost;
        }
        else if (card.GetComponent<CardVisual>().Ed != null)
        {
            return card.GetComponent<CardVisual>().Ed.GoldAndManaCost;
        }
        else
        {
            return 0;
        }
    }

    private void MoveShopCardToDiscard(GameObject card)
    {
        MoveCardCommand mc = new MoveCardCommand(card, GameManager.Instance.GetActiveDiscardPile(true), UIManager.Instance.GetActiveDiscardList(true));
        mc.AddToQueue();
    }

    private void MoveShopCardToHand(GameObject card)
    {
        MoveCardCommand mc = new MoveCardCommand(card, GameManager.Instance.GetActiveHand(true), null);
        mc.AddToQueue();
        //GameObject shopCard = GameManager.Instance.MoveCard(selectedCard, GameManager.Instance.GetActiveHand(true), null, true);
        //shopCard.AddComponent<PlayCard>();
        card.GetComponent<CardVisual>().inShop = false;
        UIManager.Instance.GetActiveHandList(true).Add(card.GetComponent<CardVisual>().CardData);
        GameManager.Instance.DisableExpressBuy();
        //EventManager.Instance.PostNotification(EVENT_TYPE.POWER_EXPRESS_BUY);
        EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_EXPRESS_BUY);
    }

    private void DelayOnPile()
    {
        string type = selectedCard.GetComponent<CardVisual>().Md.CardClass;
        DelayCommand dc;

        if (type == "Warrior")
        {
            dc = new DelayCommand(warriorDeck, 1f);
        }
        else if (type == "Rogue")
        {
            dc = new DelayCommand(rogueDeck, 1f);
        }
        else
        {
            dc = new DelayCommand(mageDeck, 1f);
        }

        dc.AddToQueue();
    }
}
