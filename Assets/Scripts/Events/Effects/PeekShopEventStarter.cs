using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PeekShopEventStarter : MonoBehaviour
{
    public List<MinionData> moveCards = new List<MinionData>();
    private GameObject card;

    const byte PEEK_SHOP_SYNC_EVENT = 44;

    public void StartEvent()
    {
        StartCoroutine(GameManager.Instance.SetInstructionsText("Peek Shop Effect"));

        card = gameObject;

        //Disable features in Shop
        DisableShop();

        //Show the Shop
        GameManager.Instance.shop.gameObject.SetActive(true);
    }

    public void DisableShop()
    {
        //Remove ShowShopCardScripts
        AddDestroyShowShop(GameManager.Instance.warriorShopPile, true);
        AddDestroyShowShop(GameManager.Instance.rogueShopPile, true);
        AddDestroyShowShop(GameManager.Instance.mageShopPile, true);
        AddDestroyShowShop(GameManager.Instance.essentialsPile, true);

        //Disable Change and Buy Buttons
        GameManager.Instance.buyButton.gameObject.SetActive(false);
        GameManager.Instance.changeButton.gameObject.SetActive(false);
        GameManager.Instance.exitShopButton.gameObject.SetActive(false);
        GameManager.Instance.moveButton.gameObject.SetActive(true);

        //Add PeekShopListener scripts
        GameManager.Instance.shop.GetComponent<ShopController>().warriorDeck.gameObject.AddComponent<PeekShopListener>();
        GameManager.Instance.shop.GetComponent<ShopController>().rogueDeck.gameObject.AddComponent<PeekShopListener>();
        GameManager.Instance.shop.GetComponent<ShopController>().mageDeck.gameObject.AddComponent<PeekShopListener>();

        //Add class type to the scripts
        GameManager.Instance.shop.GetComponent<ShopController>().warriorDeck.gameObject.GetComponent<PeekShopListener>().minionClass = "Warrior";
        GameManager.Instance.shop.GetComponent<ShopController>().rogueDeck.gameObject.GetComponent<PeekShopListener>().minionClass = "Rogue";
        GameManager.Instance.shop.GetComponent<ShopController>().mageDeck.gameObject.GetComponent<PeekShopListener>().minionClass = "Mage";

        //Add current card to script
        GameManager.Instance.shop.GetComponent<ShopController>().warriorDeck.gameObject.GetComponent<PeekShopListener>().card = this.card;
        GameManager.Instance.shop.GetComponent<ShopController>().rogueDeck.gameObject.GetComponent<PeekShopListener>().card = this.card;
        GameManager.Instance.shop.GetComponent<ShopController>().mageDeck.gameObject.GetComponent<PeekShopListener>().card = this.card;

        //Delete all GameObjects inside of the BigCard View in the shop
        GameObject cardGroup = GameManager.Instance.shop.gameObject.GetComponent<ShopController>().cardGroup;

        foreach (Transform tmp in cardGroup.transform)
        {
            Destroy(tmp.gameObject);
        }
    }

    public void EnableShop()
    {
        //Remove ShowShopCardScripts
        AddDestroyShowShop(GameManager.Instance.warriorShopPile, false);
        AddDestroyShowShop(GameManager.Instance.rogueShopPile, false);
        AddDestroyShowShop(GameManager.Instance.mageShopPile, false);
        AddDestroyShowShop(GameManager.Instance.essentialsPile, false);

        //Disable Change and Buy Buttons
        GameManager.Instance.buyButton.gameObject.SetActive(true);
        GameManager.Instance.changeButton.gameObject.SetActive(true);
        GameManager.Instance.exitShopButton.gameObject.SetActive(true);
        GameManager.Instance.moveButton.gameObject.SetActive(false);
        GameManager.Instance.moveButton.interactable = false;
        GameManager.Instance.shop.gameObject.SetActive(false);
    }

    public void DestroyPeekShopListener()
    {
        //Destroy PeekShopListener scripts
        Destroy(GameManager.Instance.shop.GetComponent<ShopController>().warriorDeck.gameObject.GetComponent<PeekShopListener>());
        Destroy(GameManager.Instance.shop.GetComponent<ShopController>().rogueDeck.gameObject.GetComponent<PeekShopListener>());
        Destroy(GameManager.Instance.shop.GetComponent<ShopController>().mageDeck.gameObject.GetComponent<PeekShopListener>());
    }

    private void AddDestroyShowShop(Transform pile, bool destroy)
    {
        foreach (Transform t in pile)
        {
            if (destroy)
            {
                ShowShopCard ssc = t.gameObject.GetComponent<ShowShopCard>();
                Destroy(ssc);
            }
            else
            {
                t.gameObject.AddComponent<ShowShopCard>();
            }
        }
    }

    public void MoveCardsToBottom()
    {
        //Move Selected Cards to the bottom of the deck
        if (moveCards.Count != 0)
        {
            UIManager.Instance.MoveTopCardsToBottom(moveCards.First().CardClass, moveCards);
        }

        //Delete all GameObjects inside of the BigCard View in the shop
        GameObject cardGroup = GameManager.Instance.shop.gameObject.GetComponent<ShopController>().cardGroup;

        foreach (Transform tmp in cardGroup.transform)
        {
            Destroy(tmp.gameObject);
        }

        //Re-enable the shop view
        EnableShop();

        //Call the effect on the other screen
        List<int> minionList = new List<int>();
        foreach (MinionData minion in moveCards)
        {
            minionList.Add(minion.MinionID);
        }

        object[] data = new object[] { moveCards.First().CardClass, minionList };
        PhotonNetwork.RaiseEvent(PEEK_SHOP_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);

        //Delete all objects in moveCards
        moveCards.Clear();

        //Call Peek Shop Power Effect 
        //EventManager.Instance.PostNotification(EVENT_TYPE.POWER_PEEK_SHOP);
        EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_PEEK_SHOP);

        //Call the Next Power in the Queue
        InvokeEventCommand.InvokeNextEvent();

        UIManager.Instance.RemoveEffectIcon = true;
    }
}
