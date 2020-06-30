using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashController : MonoBehaviour
{
    public Transform discardPile;
    public Transform handPile;
    public Button removeButton;

    private Card cardToRemove;
    public GameObject card;
    public Card CardToRemove { get => cardToRemove; set => cardToRemove = value; }
    public GameObject Card { get => card; set => card = value; }

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
                card = GameManager.Instance.SpawnCard(handPile, null, null, cv.Sd);
            }
            else
            {
                card = GameManager.Instance.SpawnCard(handPile, null, cv.Ed);
            }

            card.AddComponent<TrashListener>();
            card.GetComponent<TrashListener>().location = "hand";
            card.GetComponent<TrashListener>().index = i;
            i++;
        }
    }
    
    public void LoadDiscardList()
    {
        List<GameObject> discardCards = GameManager.Instance.GetActiveDiscardPileList(true);

        //for (int i = 0; i < discardCards.Count; i++)
        //{
        //    GameObject card = Instantiate(discardCards[i]);
        //    card.transform.SetParent(discardPile, false);
        //    card.AddComponent<TrashListener>();
        //}
        int i = 0;
        foreach (Card c in UIManager.Instance.GetActiveDiscardList(true))
        {
            GameObject card;

            if (c is MinionData)
            {
                card = GameManager.Instance.SpawnCard(discardPile, (MinionData)c);
            }
            else if (c is StarterData)
            {
                card = GameManager.Instance.SpawnCard(discardPile, null, null, (StarterData)c);
            }
            else if (c is EssentialsData)
            {
                card = GameManager.Instance.SpawnCard(discardPile, null, (EssentialsData)c);
            }
            else
            {
                card = null;
            }

            card.AddComponent<TrashListener>();
            card.GetComponent<TrashListener>().location = "discard";
            card.GetComponent<TrashListener>().index = i;
            i++;
        }
    }

    public void OnDisable()
    {
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
        //Remove From Hand
        if (UIManager.Instance.GetActiveHandList(true).Contains(cardToRemove))
        {
            UIManager.Instance.GetActiveHandList(true).Remove(cardToRemove);
        }

        //Remove from DiscardList
        if (UIManager.Instance.GetActiveDiscardList(true).Contains(cardToRemove))
        {
            UIManager.Instance.GetActiveDiscardList(true).Remove(cardToRemove);
        }
        
        //Remove actual Card from hand
        if (card.GetComponent<TrashListener>().location == "hand")
        {
            Transform tmp = GameManager.Instance.GetActiveHand(true).GetChild(card.GetComponent<TrashListener>().index);
            Destroy(tmp.gameObject);
        }

        //Remove actual Card from discard
        if (card.GetComponent<TrashListener>().location == "discard")
        {
            Transform tmp = GameManager.Instance.GetActiveHand(true).GetChild(card.GetComponent<TrashListener>().index);
            Destroy(tmp.gameObject);
        }

        gameObject.SetActive(false);
    }
}
