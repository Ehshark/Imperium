﻿using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    //Start Game Variables
    private int hostChoice; //1 = Warrior, 2 = Rogue, 3 = Mage
    private int clientChoice; //1 = Warrior, 2 = Rogue, 3 = Mage
    private bool hostFirst;
    public int HostChoice { get => hostChoice; set => hostChoice = value; }
    public int ClientChoice { get => clientChoice; set => clientChoice = value; }
    public bool HostFirst { get => hostFirst; set => hostFirst = value; }


    //public access to instance
    public static EventManager Instance { get; private set; } = null;

    //Array of listeners (all objects registered for events)
    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>();

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Function to add listener to array of listeners
    public void AddListener(EVENT_TYPE Event_Type, IListener Listener)
    {
        //Check existing event type key. If exists, add to list
        if (Listeners.TryGetValue(Event_Type, out List<IListener> ListenList))
        {
            //List exists, so add new item
            ListenList.Add(Listener);
            return;
        }

        //Otherwise create new list as dictionary key
        ListenList = new List<IListener>
        {
            Listener
        };
        Listeners.Add(Event_Type, ListenList);
    }

    public void PostNotification(EVENT_TYPE Event_Type)
    {
        //Notify all listeners of an event

        //If no event exists, then exit
        if (!Listeners.TryGetValue(Event_Type, out List<IListener> ListenList))
            return;

        //Entry exists. Now notify appropriate listeners
        for (int i = 0; i < ListenList.Count; i++)
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_Type);
    }

    //Remove event from dictionary, including all listeners
    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        //Remove entry from dictionary
        Listeners.Remove(Event_Type);
    }

    //Remove all redundant entries from the Dictionary
    public void RemoveRedundancies()
    {
        //Create new dictionary
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners = new Dictionary<EVENT_TYPE, List<IListener>>();

        //Cycle through all dictionary entries
        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            //Cycle all listeners, remove null
            for (int i = Item.Value.Count - 1; i >= 0; i--)
                //if null then remove item
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);

            //If items remain in list, then add to tmp dictionary
            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);

            //Replace listeners object with new dictionary
            Listeners = TmpListeners;
        }
    }

    //Following function is deprecated
    //private void OnLevelWasLoaded()
    //{
    //    RemoveRedundancies();
    //}

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    //Called on scene change. Clean up dictionary
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        RemoveRedundancies();
    }
    //Multiplayer: Sync changes to shop
    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 21) //sync buy card
        {
            Transform shopPile;
            object[] data = (object[])photonEvent.CustomData;
            string cardType = (string)data[0];
            int id = (int)data[1];
            string to = (string)data[2];

            if (cardType.Equals("Warrior"))
                shopPile = GameManager.Instance.warriorShopPile;
            else if (cardType.Equals("Rogue"))
                shopPile = GameManager.Instance.rogueShopPile;
            else
                shopPile = GameManager.Instance.mageShopPile;

            foreach (Transform child in shopPile)
            {
                CardVisual cv = child.GetComponent<CardVisual>();
                if (cv != null && cv.Md.MinionID == id)
                {
                    GameManager.Instance.topHero.AdjustGold(cv.Md.GoldAndManaCost, false);
                    GameManager.Instance.shop.GetComponent<ShopController>().SpawnShopMinion(cardType, false);

                    if (to.Equals("Discard"))
                        GameManager.Instance.MoveCard(child.gameObject, GameManager.Instance.enemyDiscardPile, UIManager.Instance.enemyDiscards);
                    else
                    {
                        GameObject card = GameManager.Instance.MoveCard(child.gameObject, GameManager.Instance.enemyHand, UIManager.Instance.enemyHand, true);
                        card.GetComponent<CardVisual>().gameObject.SetActive(false);
                        card.GetComponent<CardVisual>().gameObject.SetActive(true);
                        card.transform.Find("CardBack").gameObject.SetActive(true);
                    }
                }
            }

            if (GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard != null)
            {
                Destroy(GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard.gameObject);
                GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard = null;
            }
        }
        else if (eventCode == 22) //sync change shop
        {
            Transform shopPile;
            object[] data = (object[])photonEvent.CustomData;
            string cardType = (string)data[0];
            int id = (int)data[1];

            if (cardType.Equals("Warrior"))
                shopPile = GameManager.Instance.warriorShopPile;
            else if (cardType.Equals("Rogue"))
                shopPile = GameManager.Instance.rogueShopPile;
            else
                shopPile = GameManager.Instance.mageShopPile;

            foreach (Transform child in shopPile)
            {
                CardVisual cv = child.GetComponent<CardVisual>();
                if (cv != null && cv.Md.MinionID == id)
                {
                    GameManager.Instance.topHero.AdjustGold(int.Parse(cv.cost.text) / 2, false);
                    GameManager.Instance.shop.GetComponent<ShopController>().SpawnShopMinion(cardType, true, cv.Md);
                    Destroy(child.gameObject);
                }
            }

            if (GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard != null)
            {
                Destroy(GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard.gameObject);
                GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard = null;
            }
        }

        else if (eventCode == 28) //Recycle Sync
        {
            object[] data = (object[])photonEvent.CustomData;
            int index = (int)data[0];
            CardVisual cv = GameManager.Instance.enemyDiscardPile.GetChild(index).GetComponent<CardVisual>();
            UIManager.Instance.enemyDeck.Insert(0, cv.GetCardData());
            TMP_Text deckCounter = GameManager.Instance.enemyDeck.transform.Find("DeckCounter").GetComponent<TMP_Text>();
            deckCounter.text = UIManager.Instance.enemyDeck.Count.ToString();
            Destroy(GameManager.Instance.enemyDiscardPile.GetChild(index).gameObject);
            UIManager.Instance.enemyDiscards.RemoveAt(index);
        }

        else if (eventCode == 30) //Trash sync
        {
            object[] data = (object[])photonEvent.CustomData;
            int handIndex = (int)data[0];
            int discardIndex = (int)data[1];
            int destroyIndex = (int)data[2];
            if (handIndex > -1)
            {
                UIManager.Instance.enemyHand.RemoveAt(handIndex);
                Destroy(GameManager.Instance.enemyHand.GetChild(destroyIndex).gameObject);
            }
            else
            {
                UIManager.Instance.enemyDiscards.RemoveAt(discardIndex);
                Destroy(GameManager.Instance.enemyDiscardPile.GetChild(destroyIndex).gameObject);
            }
        }
        else if (eventCode == 31) //Untap sync
        {
            object[] data = (object[])photonEvent.CustomData;
            bool isMinion = (bool)data[0];
            int id = (int)data[1];

            if (isMinion)
            {
                foreach (Transform t in GameManager.Instance.enemyMinionZone)
                {
                    CardVisual cv = t.GetComponent<CardVisual>();
                    if (cv.Md.MinionID == id)
                    {
                        cv.IsTapped = false;
                        cv.ChangeTappedAppearance();
                    }
                }
            }
            else {
                foreach (Transform t in GameManager.Instance.enemyMinionZone)
                {
                    CardVisual cv = t.GetComponent<CardVisual>();
                    if (cv.Sd.StarterID == id)
                    {
                        cv.IsTapped = false;
                        cv.ChangeTappedAppearance();
                    }
                }
            }
        }
        else if (eventCode == 42)
        {
            object[] data = (object[])photonEvent.CustomData;
            int id = (int)data[0];
            string to = (string)data[1];

            foreach (Transform child in GameManager.Instance.essentialsPile)
            {
                CardVisual cv = child.GetComponent<CardVisual>();
                if (cv != null && cv.Ed.Id == id)
                {
                    GameManager.Instance.topHero.AdjustGold(cv.Ed.GoldAndManaCost, false);

                    if (to.Equals("Discard"))
                        GameManager.Instance.MoveCard(child.gameObject, GameManager.Instance.enemyDiscardPile, UIManager.Instance.enemyDiscards);
                    else
                        GameManager.Instance.MoveCard(child.gameObject, GameManager.Instance.enemyHand, UIManager.Instance.enemyHand);
                }
            }

            if (GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard != null)
            {
                Destroy(GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard.gameObject);
                GameManager.Instance.shop.GetComponent<ShopController>().BigShopCard = null;
            }
        }
        else if (eventCode == 60) //Enable GameManager Effect Icon Queue
        {
            GameManager.Instance.EffectIconQueue.gameObject.SetActive(true);
        }
        else if (eventCode == 61) //Disable GameManager Effect Icon Queue
        {
            GameManager.Instance.EffectIconQueue.gameObject.SetActive(false);
        }
    }

}
