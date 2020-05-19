using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PeekShopEventStarter : MonoBehaviour, IListener
{
    public Transform warriorDeck;
    public Transform rogueDeck;
    public Transform mageDeck;

    public Button moveButton;

    public List<MinionData> moveCards = new List<MinionData>();

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.PEEK_SHOP, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
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
        moveButton.gameObject.SetActive(true);

        //Add PeekShopListener scripts
        warriorDeck.gameObject.AddComponent<PeekShopListener>();
        rogueDeck.gameObject.AddComponent<PeekShopListener>();
        mageDeck.gameObject.AddComponent<PeekShopListener>();

        //Add class type to the scripts
        warriorDeck.gameObject.GetComponent<PeekShopListener>().minionClass = "Warrior";
        rogueDeck.gameObject.GetComponent<PeekShopListener>().minionClass = "Rogue";
        mageDeck.gameObject.GetComponent<PeekShopListener>().minionClass = "Mage";

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
        moveButton.gameObject.SetActive(false);
        GameManager.Instance.shop.gameObject.SetActive(false);
    }

    public void DestroyPeekShopListener()
    {
        //Destroy PeekShopListener scripts
        Destroy(warriorDeck.gameObject.GetComponent<PeekShopListener>());
        Destroy(rogueDeck.gameObject.GetComponent<PeekShopListener>());
        Destroy(mageDeck.gameObject.GetComponent<PeekShopListener>());
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
    }
}
