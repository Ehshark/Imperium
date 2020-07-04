using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecycleListenerAssigner : MonoBehaviour
{
    public GameObject AllyDiscardList;
    private Transform DiscardPile;
    public Button recycleButton;
    private List<Card> deckList = new List<Card>();
    TMP_Text titleText;

    private void Awake()
    {
        titleText = GameManager.Instance.alliedDiscardUI.Find("DiscardListTitle/Text").GetComponent<TMP_Text>();
    }

    public void StartEvent()
    {
        GameManager.Instance.allyDiscardPileButton.interactable = false;
        GameManager.Instance.enemyDiscardPileButton.interactable = false;
        SetupRecycleUI(true);
        //show the discardpile for player
        UIManager.Instance.DisplayAllyDiscards();
        recycleButton = GameManager.Instance.recycleButton.GetComponent<Button>();
        recycleButton.onClick.AddListener(RecycleConfirmButton);
        DiscardPile = GameManager.Instance.alliedDiscardUI.transform.Find("CardPile/Cards");

        foreach (Transform t in DiscardPile)
        {
            t.gameObject.AddComponent<RecycleListener>();
        }
        //recycle button add the clicked card as reference. and put it top of the deck? 
    }

    //Adjusts the ui for discard pile and becomes a recycle ui pass true for recycle then false for turning it back to regular 
    public void SetupRecycleUI(bool on)
    {
        if (on)
        {
            //turn off exit exit attach recycle button and change title
            titleText.text = "Recycle";
            GameManager.Instance.exitDiscardsButton.gameObject.SetActive(false);
            GameManager.Instance.recycleButton.gameObject.SetActive(true);
        }
        else
        {
            //turn on exit button 
            titleText.text = "Discard";
            GameManager.Instance.exitDiscardsButton.gameObject.SetActive(true);
            GameManager.Instance.recycleButton.gameObject.SetActive(false);
        }
    }

    public void RecycleConfirmButton()
    {
        if (UIManager.Instance.LastSelectedCard)
        {
            CardVisual cv = UIManager.Instance.LastSelectedCard.GetComponent<CardVisual>();
            //move the selected card to top of the deck. 
            deckList = UIManager.Instance.GetActiveDeckList(true);
            deckList.Insert(0, cv.GetCardData());

            TMP_Text deckCounter = GameManager.Instance.GetActiveDeck(true).transform.Find("DeckCounter").GetComponent<TMP_Text>();
            deckCounter.text = deckList.Count.ToString();

            //destroy the selected card from the discard pile
            Card cardData;
            CardVisual cv2;
            foreach (Transform t in GameManager.Instance.GetActiveDiscardPile(true))
            {
                cv2 = t.gameObject.GetComponent<CardVisual>();
                cardData = cv2.GetCardData();

                if (cardData == cv.GetCardData())
                {
                    Destroy(t.gameObject);
                }
            }
        }

        GameManager.Instance.alliedDiscardUI.gameObject.SetActive(false);
        SetupRecycleUI(false);
        GameManager.Instance.allyDiscardPileButton.interactable = true;
        GameManager.Instance.enemyDiscardPileButton.interactable = true;
    }
}

