using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class TrashController : MonoBehaviour
{
    public Transform discardPile;
    public Transform handPile;
    public Button removeButton;

    private Card cardToRemove;
    public GameObject card;
    public Card CardToRemove { get => cardToRemove; set => cardToRemove = value; }
    public GameObject Card { get => card; set => card = value; }

    //Multiplayer
    const byte TRASH_SYNC_EVENT = 30;

    // Start is called before the first frame update
    void OnEnable()
    {
        LoadHandPile();
        LoadDiscardList();
        removeButton.interactable = false;
    }

    public void LoadHandPile()
    {
        Transform handCards = GameManager.Instance.GetActiveHand(true);

        int i = 0;
        foreach (Transform t in handCards)
        {
            GameObject card;

            CardVisual cv = t.GetComponent<CardVisual>();
            if (cv.Md != null)
            {
                card = GameManager.Instance.SpawnCard(handPile, cv.Md);
            }
            else if (cv.Sd != null)
            {
                card = GameManager.Instance.SpawnCard(handPile, cv.Sd);
            }
            else
            {
                card = GameManager.Instance.SpawnCard(handPile, cv.Ed);
            }

            card.AddComponent<TrashListener>();
            card.GetComponent<TrashListener>().location = "hand";
            card.GetComponent<TrashListener>().index = i;
            i++;
        }
    }

    public void LoadDiscardList()
    {
        int i = 0;
        foreach (Card c in UIManager.Instance.GetActiveDiscardList(true))
        {
            GameObject card;

            card = GameManager.Instance.SpawnCard(discardPile, c);

            card.AddComponent<TrashListener>();
            card.GetComponent<TrashListener>().location = "discard";
            card.GetComponent<TrashListener>().index = i;
            i++;
        }
    }

    public void DestroyCards()
    {
        foreach (Transform t in handPile)
        {
            Destroy(t.gameObject);
        }

        foreach (Transform t in discardPile)
        {
            Destroy(t.gameObject);
        }

        removeButton.interactable = false;
        Card = null;
        cardToRemove = null;
    }

    public void DestroyCard()
    {
        int handIndex = -1;
        int discardIndex = -1;
        //Remove From Hand
        if (UIManager.Instance.GetActiveHandList(true).Contains(cardToRemove))
        {
            handIndex = UIManager.Instance.GetActiveHandList(true).IndexOf(cardToRemove);
            UIManager.Instance.GetActiveHandList(true).Remove(cardToRemove);
        }

        //Remove from DiscardList
        if (UIManager.Instance.GetActiveDiscardList(true).Contains(cardToRemove))
        {
            discardIndex = UIManager.Instance.GetActiveDiscardList(true).IndexOf(cardToRemove);
            UIManager.Instance.GetActiveDiscardList(true).Remove(cardToRemove);
        }

        object[] data = new object[] { handIndex, discardIndex, card.GetComponent<TrashListener>().index };

        //Remove actual Card from hand
        if (card.GetComponent<TrashListener>().location == "hand")
        {
            PhotonNetwork.RaiseEvent(TRASH_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);
            Transform tmp = GameManager.Instance.GetActiveHand(true).GetChild(card.GetComponent<TrashListener>().index);
            Destroy(tmp.gameObject);
        }

        //Remove actual Card from discard
        if (card.GetComponent<TrashListener>().location == "discard")
        {
            PhotonNetwork.RaiseEvent(TRASH_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);
            Transform tmp = GameManager.Instance.GetActiveDiscardPile(true).GetChild(card.GetComponent<TrashListener>().index);
            Destroy(tmp.gameObject);
        }

        DestroyCards();
        gameObject.SetActive(false);

        //Call the next power or condition
        EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_TRASH);

        //Call the Next Effect in the Queue
        InvokeEventCommand.InvokeNextEvent();

        UIManager.Instance.RemoveEffectIcon = true;
    }
}
